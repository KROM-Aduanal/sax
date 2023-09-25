#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher.TypeStatus

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web


'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB


'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)


#End Region

Public Class Ges022_PlantillaSynapsis
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                 Defina en esta región sus atributos o propiedades locales                      ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

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

        'With Buscador

        '    .DataObject = New ConstructorCliente(True)

        '    .addFilter(SeccionesClientes.SCS1, CamposClientes.CA_RAZON_SOCIAL, "Cliente")

        'End With

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


    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Asocie los controles a su documento electrónico                                                                                                                '
        ' De forma automatica guardará y editara el registro en Mongo DB                                                                                                 '
        ' De forma automatica llenara el formulario al realizar una consulta en Mongo DB                                                                                 '
        ' Hay que mencionar que en ocasiones se tendra que manipular el documento de forma manual en metodos que se describiran mas adelante                             '
        ' para relacionarlos se usara el metodo [Set]; se colocara el ID del control como primer parametro y el ID del campo en nuestro documento como segundo parámetro '
        ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'EJEMPLO CONTROLES DE CAPTURA

        '[Set](t_RFC, CA_RFC_CLIENTE)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' t_RFC          = ID del control                           '
        ' CA_RFC_CLIENTE = ID del campo en el documento electrónico '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'EJEMPLO CATALOGO (aplica de la misma manera para el TARJETERO)

        '[Set](sc_claveAduanaSeccion, CP_CVE_ADUANA_SECCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](sc_patenteAduanal, CP_CVE_PATENTE_ADUANAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Asociamos los controles del catalogo al documento pero se agrega la propiedad "propiedadDelControl_:=PropiedadesControl.Ninguno"  '
        ' Esto para evitar que intente buscar este campo en las secciones que no sean de tipo partida                                       '
        ' Se procede a asociar el catalogo a la seccion de tipo partida donde almacenara su lista de datos                                  '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '[Set](_cataduanasdefecto, Nothing, seccion_:=SeccionesClientes.SCS2)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' _cataduanasdefecto     = ID del control                                                                              '
        ' omitimos el parametro del campo porque el catalogo guardara una lista de datos                                       '
        ' SeccionesClientes.SCS2 = ID de la seccion del documento donde se almacenaran los registros del catalago              '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return New TagWatcher(1)

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

        'If Not ProcesarTransaccion(Of ConstructorClientes)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicPublicar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Publicar '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Seguir Editando '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

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

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████



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

        With documentoElectronico_

            .FolioDocumento = 0

            .FolioOperacion = ""

            .IdCliente = 0

            .NombreCliente = ""

        End With

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

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



#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██ Defina en esta región todo lo que involucre el uso de controladores externos al contexto actual██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


#End Region


End Class
