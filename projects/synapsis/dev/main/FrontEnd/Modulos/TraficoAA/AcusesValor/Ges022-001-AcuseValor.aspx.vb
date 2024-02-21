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
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus

Public Class Ges022_001_AcuseValor
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                 Defina en esta región sus atributos o propiedades locales                      ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorProveedor As New CtrlProveedoresOperativos

    Private _icontroladorMonedas As IControladorMonedas
    Private _icontroladorAcuseValor As IControladorAcuseValor

    Private _icontroladorFactura As IControladorFacturaComercial

    Private _icontroladorEmpresa As New ControladorEmpresas

    Private _organismo As New Syn.Utils.Organismo

    Private _controladorUnidadesMedida As New ControladorUnidadesMedida

    Private _sistema As New Syn.Utils.Organismo
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

            .DataObject = New ConstructorAcuseValor(True)
            .addFilter(SeccionesAcuseValor.SAcuseValor1, CamposAcuseValor.CA_NUMERO_ACUSEVALOR, "ACUSE DE VALOR")
            .addFilter(SeccionesAcuseValor.SAcuseValor1, CamposFacturaComercial.CA_NUMERO_FACTURA, "FACTURA")
            .addFilter(SeccionesAcuseValor.SAcuseValor2, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, "Proveedor")
            .addFilter(SeccionesAcuseValor.SAcuseValor3, CamposDestinatario.CA_RAZON_SOCIAL, "Destinatario")
            .addFilter(SeccionesAcuseValor.SAcuseValor4, CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, "Descripción A.V.")

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

        _icontroladorAcuseValor = New ControladorAcuseValor

        _icontroladorFactura = New ControladorFacturaComercial(1, True)

        _icontroladorMonedas = New ControladorMonedas

        _controladorProveedor = New CtrlProveedoresOperativos

        SetVars("_AcuseValorFindBar", Nothing)

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher


        'Datos Generales
        'Case SeccionesACUSEVALOR.SACUSEVALOR1



        '                                    Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40),
        [Set](dbc_NumFacturaAcuseValor, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor)
        '                                    Item(CamposACUSEVALOR.CA_NUMERO_ACUSEVALOR, Texto, longitud_:=40),
        [Set](dbc_NumFacturaAcuseValor, CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        '                                    Item(CamposFacturaComercial.CP_TIPO_OPERACION, Texto, longitud_:=11),
        [Set](IIf(swc_TipoOperacion.Checked, "Importación", "Exportación"), CamposFacturaComercial.CP_TIPO_OPERACION)
        '                                    Item(CamposACUSEVALOR.CP_TIPO_DOCUMENTO_ACUSEVALOR, Texto, longitud_:=40),
        [Set](sc_TipoDocumento, CP_TIPO_DOCUMENTO_ACUSEVALOR)
        '                                    Item(CamposFacturaComercial.CA_MONEDA_FACTURACION, Texto, longitud_:=20),
        [Set](sc_TipoMoneda, CA_MONEDA_FACTURACION)
        '                                    Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha),
        [Set](icFechaExpedicion, CA_FECHA_FACTURA)
        '                                    Item(CamposACUSEVALOR.CA_FECHA_ACUSEVALOR, Fecha),
        '                                    Item(CamposFacturaComercial.CA_APLICA_SUBDIVISION, Texto, longitud_:=3),
        [Set](IIf(swc_Subdivision.Checked, "Sí", "NO"), CamposFacturaComercial.CA_APLICA_SUBDIVISION)
        '                                    Item(CamposACUSEVALOR.CA_RELACION_FACTURA_ACUSEVALOR, Texto, longitud_:=100)
        [Set](IIf(swc_RelacionFactura.Checked, "Sí", "NO"), CamposAcuseValor.CA_RELACION_FACTURA_ACUSEVALOR)
        '                                    Item(CamposFacturaComercial.CA_APLICA_CERTIFICADO, Texto, longitud_:=3),
        [Set](IIf(swc_CertificadoOrigen.Checked, "Sí", "NO"), CamposFacturaComercial.CA_APLICA_CERTIFICADO)
        '                                    Item(CamposACUSEVALOR.CA_NUMERO_EXPORTADOR_ACUSEVALOR, Texto, longitud_:=100),
        [Set](ic_ExpotadorAutorizado, CA_NUMERO_EXPORTADOR_ACUSEVALOR)
        '                                    Item(CamposACUSEVALOR.CA_OBSERVACIONES_ACUSEVALOR, Texto, longitud_:=450)
        [Set](ic_Observaciones, CA_OBSERVACIONES_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                    Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                    Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)

        'Datos del proveedor
        'Case SeccionesACUSEVALORl.SACUSEVALOR2
        '                                     Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120)
        [Set](fbc_Proveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)

        [Set](fbc_Proveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        ' [Set](ic_IDFiscalProveedor, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR)

        [Set](ic_DireccionProveedor, CamposDomicilio.CA_DOMICILIO_FISCAL)
        '                                     Item(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, Texto, longitud_:=11)
        '                                     Item(CamposProveedorOperativo.CA_RFC_PROVEEDOR, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXT_INT, Texto, longitud_:=20)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
        '                                     Item(CamposFacturaComercial.CA_CVE_VINCULACION, Entero)

        'Datos del destinatario
        'Case SeccionesACUSEVALORl.SACUSEVALOR3
        [Set](fbc_Destinatario, CamposDestinatario.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        ' [Set](ic_IDFiscalDestinatario, CamposDestinatario.CA_RFC_DESTINATARIO)
        '                                     Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto, longitud_:=120)
        [Set](ic_DireccionDestinatario, CamposDomicilio.CA_DOMICILIO_FISCAL)
        '                                     Item(CamposDestinatario.CA_TAX_ID, Texto, longitud_:=11)
        '                                     Item(CamposDestinatario.CA_RFC_DESTINATARIO, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)

        ' Partidas - Factura-ACUSEVALOR
        'Case SeccionesACUSEVALOR.SACUSEVALOR4
        '                                     Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero)
        '[Set](lbNumeroACUSEVALOR, CP_NUMERO_PARTIDA)
        If pb_PartidasAcuseValor.PageIndex > 0 Then

            lbNumeroAcuseValor.Text = pb_PartidasAcuseValor.PageIndex.ToString()

        End If

        '                                     Item(CamposACUSEVALOR.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, Texto, longitud_:=250),

        [Set](ic_DescripcionAcuseValor, CA_DESCRIPCION_PARTIDA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
        [Set](ic_PrecioUnitarioAcuseValor, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
        [Set](ic_CantidadAcuseValor, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposACUSEVALOR.CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR, Texto, longitud_:=80),
        [Set](sc_UnidadAcuseValor, CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Texto, longitud_:=3)
        '                                     Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=80),
        [Set](sc_MonedaPrecioUnitarioPartida, CP_MONEDA_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
        [Set](ic_ValorFacturaPartida, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposACUSEVALOR.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
        [Set](ic_ValorDolaresPartida, CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        ' Partida - detalle mercancía
        '                                     Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
        [Set](ic_MarcaAcuseValor, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
        [Set](ic_ModeloAcuseValor, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
        [Set](ic_SubmodeloAcuseValor, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=80)
        [Set](ic_NumeroSerieAcuseValor, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pb_PartidasAcuseValor, Nothing, seccion_:=SeccionesAcuseValor.SAcuseValor4)
        ' Configuración
        'Case SeccionesACUSEVALOR.SACUSEVALOR5
        '                                     Item(CamposACUSEVALOR.CA_SELLO_ACUSEVALOR, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](sc_SelloCliente, CA_SELLO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposACUSEVALOR.CA_RFC_CONSULTA_ACUSEVALOR, Texto, longitud_:=3)
        [Set](ic_RFCSConsulta, CA_RFC_CONSULTA_ACUSEVALOR)
        '                                     Item(CamposACUSEVALOR.CA_PATENTE_ACUSEVALOR, Texto, longitud_:=)
        [Set](ic_PatenteAduanal, CA_PATENTE_ACUSEVALOR)
        '                                     Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3)

        '                                     Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3)


        Return New TagWatcher(Ok)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción nuevo (+) '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        sc_TipoDocumento.Value = "1"

        icFechaExpedicion.Value = DateTime.UtcNow.Date.ToString("yyyy-MM-dd")

        SetVars("_sc_tipomoneda", sc_TipoMoneda)

        Dim listaSelectOption_ = New List(Of SelectOption)

        Dim monedas_ = _icontroladorMonedas.BuscarMonedas(New List(Of String) From {"USD",
                                                                                    "EUR",
                                                                                    "MXP",
                                                                                    "CHF",
                                                                                    "JPY",
                                                                                    "CNY"},,
                                                                                    "cveAcuseValor")


        listaSelectOption_ = _organismo.ObtenerSelectOption(sc_TipoMoneda,
                                                            monedas_.Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                            .Id = e._id,
                            .Valor = e.nombremonedaesp & " | " & e.aliasmoneda.Find(Function(ef) ef.Clave = "cveAcuseValor").Valor
                           }).ToList)

        SetVars("_Monedas", monedas_)

        sc_TipoMoneda.DataSource = listaSelectOption_

        sc_MonedaPrecioUnitarioPartida.DataSource = listaSelectOption_

        sc_TipoMoneda.Value = listaSelectOption_(0).Value

        sc_MonedaPrecioUnitarioPartida.Value = listaSelectOption_(0).Value

        listaSelectOption_ = _organismo.ObtenerSelectOption(sc_UnidadAcuseValor,
                                                            _controladorUnidadesMedida.
                                                            BuscarUnidadesCOVE(New List(Of String) From {"C62_1",
                                                                                                         "KGM",
                                                                                                         "CS",
                                                                                                         "SET",
                                                                                                         "C62_2",
                                                                                                         "KT",
                                                                                                         "TNE",
                                                                                                         "LM",
                                                                                                         "MIL",
                                                                                                         "MQ",
                                                                                                         "MTK",
                                                                                                         "BX",
                                                                                                         "LTR",
                                                                                                         "GRM"}).
                                                                                                         Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                                                                                                              .Id = e._id,
                                                                                                              .Valor = e.nombreoficialesp
                                                                                                          }).ToList)

        sc_UnidadAcuseValor.DataSource = listaSelectOption_

        sc_UnidadAcuseValor.Value = listaSelectOption_(0).Value

        sc_TipoMoneda.ToolTip = "Factor: $" &
                                 monedas_(0).factoresmoneda(0).valordefault &
                                 " al " &
                                 monedas_(0).factoresmoneda(0).fecha.ToString("dd-MM-yyyy")

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pb_PartidasAcuseValor)

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Este método se manda llamar al dar clic en el boton Guardar                             '
        ' Llamamos el método "ProcesarTransaccion" pasando el tipo de nuestra clase constructora  '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim acuseValorFindBar_ As ConstructorAcuseValor = GetVars("_AcuseValorFindBar")

        Dim sinCambio_ As Boolean

        If acuseValorFindBar_ IsNot Nothing Then

            If acuseValorFindBar_.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor Is Nothing Then

                sinCambio_ = True

            Else

                DisplayMessage("Este Documento ya tenía el Número de AcuseValor '" &
                               acuseValorFindBar_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                               Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor &
                               " Por lo que se procederá a hacer una Adenda", StatusMessage.Info)

                DisplayMessage("Este Documento ya tenía el Número de AcuseValor '" &
                               acuseValorFindBar_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                               Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor &
                               " Por lo que se procederá a hacer una Adenda", StatusMessage.Info)

            End If

        End If

        If Not ProcesarTransaccion(Of ConstructorAcuseValor)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicPublicar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Publicar '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''

        With Buscador

            Dim Algo = GetVars("_AcuseValorFindBar")

            Dim TagWatcher_ = _icontroladorAcuseValor.GenerarAcuseValor(GetVars("_AcuseValorFindBar"))

            If TagWatcher_.ObjectReturned <> "" Then

                DisplayMessage("SU ACUSE DE VALOR HA SIDO GENERADO", StatusMessage.Info)

                dbc_NumFacturaAcuseValor.ValueDetail = TagWatcher_.ObjectReturned

            End If

        End With

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Seguir Editando '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pb_PartidasAcuseValor)

        PreparaBotonera(FormControl.ButtonbarModality.Draft)

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
        'End If
        Select Case IndexSelected_

            Case 7

                If dbc_NumFacturaAcuseValor.ValueDetail = "" Then

                    DisplayMessage("No hay Acuse de Valor generado para adendar", StatusMessage.Fail)

                Else

                    With Buscador

                        Dim TagWatcher_ = _icontroladorAcuseValor.GenerarAcuseValor(GetVars("_AcuseValorFindBar"), True)

                        If TagWatcher_.ObjectReturned <> "" Then

                            DisplayMessage("SU ACUSE DE VALOR HA SIDO ADENDADO", StatusMessage.Info)

                            dbc_NumFacturaAcuseValor.ValueDetail = TagWatcher_.ObjectReturned

                        End If

                    End With


                End If

            Case 8

                LimpiarTodo()

        End Select

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local
            '████████████████████████
            Dim idFactura_ = GetVars("IDS")

            If idFactura_ Is Nothing Then

                idFactura_ = ""

            End If

            If idFactura_.ToString = "" Then

                tagwatcher_ = _icontroladorFactura.
                              ListaCamposFacturaComercial(dbc_NumFacturaAcuseValor.Value.ToString,
                                                          New Dictionary(Of [Enum], List(Of [Enum])) _
                                                          From {{SeccionesFacturaComercial.SFAC1,
                                                          New List(Of [Enum]) From
                                                          {CamposFacturaComercial.CA_NUMERO_FACTURA}}})

                Dim resultado_ As Dictionary(Of String, List(Of Nodo)) = tagwatcher_.ObjectReturned

                If resultado_(dbc_NumFacturaAcuseValor.Value.ToString).Count > 0 Then

                    idFactura_ = DirectCast(resultado_(dbc_NumFacturaAcuseValor.Value.ToString).
                                                       Item(1),
                                                       Campo).
                                                       Valor

                Else

                    idFactura_ = ObjectId.GenerateNewId.ToString

                End If

            End If

            [Set](New ObjectId(idFactura_.ToString), CP_ID_FACTURA_ACUSEVALOR)

            [Set](New ObjectId(fbc_Proveedor.Value.ToString), CP_ID_Proveedor_ACUSEVALOR)

            [Set](New ObjectId(fbc_Destinatario.Value.ToString), CP_ID_Destinatario_ACUSEVALOR)


            Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

            [Set](loginUsuario_("WebServiceUserID"), CP_EMAIL_CONSULTA_ACUSEVALOR)

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

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim documentoElectronicoaux_ As DocumentoElectronico = GetVars("_ProveedorAcuseValor")

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        With documentoElectronico_

            .FolioDocumento = dbc_NumFacturaAcuseValor.Value

            .FolioOperacion = sec_

            .Id = respuesta_.ObjectReturned._id.ToString

            With .Seccion(SeccionesAcuseValor.SAcuseValor2)

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor IsNot Nothing Then

                    .Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor IsNot Nothing Then

                    .Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CALLE).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CALLE).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_COLONIA).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_COLONIA).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CIUDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CIUDAD).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CVE_PAIS).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesProvedorOperativo.SPRO2).Campo(CamposDomicilio.CA_PAIS).Valor

                End If

            End With

            documentoElectronicoaux_ = GetVars("_ClienteAcuseValor")

            With .Seccion(SeccionesAcuseValor.SAcuseValor3)

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                    .Campo(CamposDestinatario.CA_TAX_ID).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                    .Campo(CamposDestinatario.CA_RFC_DESTINATARIO).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CALLE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                End If


                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_COLONIA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CIUDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CIUDAD).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                End If

                If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                    .Campo(CamposDomicilio.CA_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor

                End If

            End With

            .NombreCliente = documentoElectronicoaux_.NombreCliente

            With .Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION)

                .Valor = sc_TipoMoneda.Value

                .ValorPresentacion = sc_TipoMoneda.Text

            End With

            .IdCliente = documentoElectronicoaux_.IdCliente

        End With

        SetVars("_AcuseValorFindBar", New ConstructorAcuseValor(True, documentoElectronico_))

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

        Dim operacionGenerica_ As OperacionGenerica = GetVars("_operacionGenerica")

        operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = operacionGenerica_.Id.ToString

        SetVars("_AcuseValorFindBar", New ConstructorAcuseValor(True, operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente))
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
    'Dim illenamonedas As Int32 = 2


    Public Sub dbc_NumFacturaAcuseValor_Click(sender As Object, e As EventArgs)


        '_organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {New ObjectId("64fb6e1ebd6d1ecd88701663")},
        '                                        New ConstructorTIGIE(), New Dictionary(Of [Enum], List(Of [Enum])) From
        '                                        {{SeccionesTarifaArancelaria.TIGIE1, New List(Of [Enum]) From
        '                                        {CamposTarifaArancelaria.CA_NUMERO_FRACCION_ARANCELARIA,
        '                                        CamposTarifaArancelaria.CA_NUMERO_NICO,
        '                                        CamposTarifaArancelaria.CA_FRACCION_ARANCELARIA}},
        '                                        {SeccionesTarifaArancelaria.TIGIE6, Nothing},
        '                                        {SeccionesTarifaArancelaria.TIGIE19, Nothing}})
        'Dim algo3_ = Nothing
        '    Dim toolTip1 = New ToolTip()

        '// Set up the delays for the ToolTip.
        'toolTip1.AutoPopDelay = 5000;
        'toolTip1.InitialDelay = 1000;
        'toolTip1.ReshowDelay = 500;
        '// Force the ToolTip text to be displayed whether Or Not the form Is active.
        'toolTip1.ShowAlways = True;

        '// Set up the ToolTip text for the Button And Checkbox.
        'toolTip1.SetToolTip(this.button1, "My button1");
        'toolTip1.SetToolTip(this.checkBox1, "My checkBox1");

        'If _ControladorMonedas Is Nothing Then
        '    _ControladorMonedas = New ControladorMonedas
        'End If
        'Dim Allgo = _ControladorMonedas.ObtenerFactorTipodeCambio("USD", Date.Parse("26/06/2023")).ObjectReturned
        'MsgBox("AHH")
        Dim ctrlBanco_ As IControladorInstitucionBancaria = New ControladorInstitucionBancaria

        'Dim Algo = ctrlBanco_.BuscarBancos(New Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object) From
        '                                    {{IControladorInstitucionBancaria.CamposBusquedaSimple.RAZONSOCIAL,
        '                                    "BBVA".ToUpper}, {IControladorInstitucionBancaria.CamposBusquedaSimple.CLAVEUSOBUSCAACTUALIZA,
        '                                    "012"}, {IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIALBUSCAACTUALIZA, "BANCOMER"},
        '                                    {IControladorInstitucionBancaria.CamposBusquedaSimple.METADATOS, "NO"}},
        '                                     IControladorInstitucionBancaria.Modalidades.Externo)
        'Dim Algo = ctrlBanco_.BuscarBancos(New Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object) From
        '                                    {{IControladorInstitucionBancaria.CamposBusquedaSimple.METADATOS, "NO"}},
        '                                     IControladorInstitucionBancaria.Modalidades.Externo)
        'MsgBox(Algo.LastMessage)
        Dim institucionBancaria_ As New InstitucionBancaria With {._id = New ObjectId("64ff2986540f5b42dd4032dc"),
                                                             ._idinstitucionbancaria = 1,
                                                             ._idempresa = ObjectId.GenerateNewId,
                                                             ._iddomicilio = ObjectId.GenerateNewId,
                                                             .uso = New List(Of UsoIdentificador) From
                                                                            {New UsoIdentificador With {.clave = "49992",
                                                                                                       .info = "Banxico"},
                                                                            New UsoIdentificador With {.clave = "992",
                                                                                                        .info = "SAT"}},
                                                             .otrosaliasinstitucion = New List(Of AliasBancos) From
                                                                                     {New AliasBancos With {.tipoalias = "Comercial",
                                                                                                            .valor = "CITIBANAMEX"},
                                                                                     New AliasBancos With {.tipoalias = "Corto",
                                                                                                            .valor = "BANAMEX"},
                                                                                     New AliasBancos With {.tipoalias = "Siglas",
                                                                                                            .valor = "BNM"},
                                                                                     New AliasBancos With {.tipoalias = "Abreviatura",
                                                                                                            .valor = "BANX"}
                                                                                                            },
                                                             .razonsocialespaniol = "BancA Racional de México, S.A., Institución de Banca Múltiple, Grupo Financiero Banamex ".ToUpper,
                                                             .domiciliofiscal = "Actuario Roberto Medellín No. 800, 2° piso Norte, Colonia Santa Fe Peña Blanca, C.P. 01210, Delegación Álvaro Obregón en México, D.F",
                                                             .metadatos = New List(Of AliasBancos) From
                                                                                     {New AliasBancos With {.tipoalias = "Pece",
                                                                                                            .valor = "SI"}
                                                                                                            },
                                                             .tipobanco = TiposBanco.Nacional,
                                                             .estado = 1,
                                                             .archivado = False
                                                                }

        'Dim Algo2 = ctrlBanco_.ActualizaBanco(New ObjectId("64f8b17a5f133141112719c6"), New Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object) From
        '                                    {{IControladorInstitucionBancaria.CamposBusquedaSimple.RAZONSOCIAL,
        '                                    "BBVA Bancomer, S.A., Institución de Banca Múltiple, Grupo Financiero BBVA Bancomer".ToUpper},
        '                                    {IControladorInstitucionBancaria.CamposBusquedaSimple.CLAVEUSONUEVO,
        '                                    New List(Of UsoIdentificador) From
        '                                                                    {New UsoIdentificador With {.clave = "400012",
        '                                                                                               .info = "Banxico"},
        '                                                                    New UsoIdentificador With {.clave = "012",
        '                                                                                                .info = "SAT"}}},
        '                                    {IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIALNUEVO,
        '                                    New AliasBancos With {.tipoalias = "Abreviatura", .valor = "BBVA"}},
        '                                    {IControladorInstitucionBancaria.CamposBusquedaSimple.DOMICILIOFISCAL,
        '                                    "Av. Paseo de la Reforma 510, Colonia Juárez, Delegación Cuauhtémoc, C.P. 06600, Ciudad de México"}
        '                                    })

        Dim Allgo2 = ctrlBanco_.NuevoBanco(institucionBancaria_)

        Dim Allgo = _icontroladorMonedas.ObtenerFactorTipodeCambio("EUR", Date.Parse("15/09/2023"))

        'Dim seccionFac_ = SeccionesFacturaComercial.SFAC4



        'Dim nodosFac_ = New List(Of [Enum]) From {CamposFacturaComercial.CP_NUMERO_PARTIDA, CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA}



        'Dim secciones_ = New Dictionary(Of [Enum], List(Of [Enum])) From {{seccionFac_, Nothing}}



        '_ControladorFactura = New ControladorFacturaComercial(1, True)



        'Dim listaValores = _ControladorFactura.ListaCamposFacturaComercial("0155524864", secciones_)

        'Dim Algo = _ControladorMonedas.TiposdeCambio(0)
        'Dim Algo3 = _ControladorMonedas.FactoresCambioRecientes(0)

        If Not Checallenado() Then

            If dbc_NumFacturaAcuseValor.Value.ToString <> "" Then


                'Dim tagwatcher3_ = _ControladorFactura.ListaCamposFacturaComercial(New ObjectId("64e7cc2b4c203fa0dcb2124a"),
                '                                                                  New Dictionary(Of [Enum], List(Of [Enum])) _
                '        From {{SeccionesFacturaComercial.SFAC4, Nothing}})
                Dim tagwatcher_ = _icontroladorFactura.ListaCamposFacturaComercial(dbc_NumFacturaAcuseValor.Value.ToString,
                                                                                  New Dictionary(Of [Enum], List(Of [Enum])) _
                        From {{SeccionesFacturaComercial.SFAC1, New List(Of [Enum]) From {CamposFacturaComercial.CA_NUMERO_FACTURA,
                                                                                          CamposFacturaComercial.CA_FECHA_FACTURA,
                                                                                          CamposFacturaComercial.CP_TIPO_OPERACION,
                                                                                          CamposClientes.CA_RAZON_SOCIAL,
                                                                                          CamposClientes.CA_TAX_ID,
                                                                                          CamposClientes.CA_RFC_CLIENTE,
                                                                                          CamposFacturaComercial.CA_MONEDA_FACTURACION,
                                                                                          CamposFacturaComercial.CP_APLICA_ENAJENACION,
                                                                                          CamposFacturaComercial.CA_APLICA_SUBDIVISION
                                }},
                                {SeccionesFacturaComercial.SFAC2, New List(Of [Enum]) From {CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR,
                                                                                            CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR,
                                                                                            CamposProveedorOperativo.CA_RFC_PROVEEDOR
                                }},
                                {SeccionesFacturaComercial.SFAC4, Nothing}})

                Dim resultado_ As Dictionary(Of String, List(Of Nodo)) = tagwatcher_.ObjectReturned


                If resultado_ IsNot Nothing Then

                    Dim listaResultado_ = resultado_(dbc_NumFacturaAcuseValor.Value.ToString)

                    ' icFechaExpedicion.Value = Date.Parse(listaResultado_("CA_FECHA_FACTURA")("Valor")).ToString("yyyy-MM-dd")

                    icFechaExpedicion.Value = Date.Parse(DirectCast(listaResultado_.Item(1), Campo).Valor).ToString("yyyy-MM-dd")

                    '                    If listaResultado_("CP_TIPO_OPERACION")("Valor") = "Importación" Then
                    If DirectCast(listaResultado_.Item(2), Campo).Valor = "Importación" Then

                        swc_TipoOperacion.Checked = True

                    Else

                        swc_TipoOperacion.Checked = False

                    End If

                    Dim idFactura_ = DirectCast(listaResultado_.Item(13), Campo).ValorPresentacion

                    SetVars("IDS", idFactura_)

                    Dim idMoneda = ObjectId.Parse(DirectCast(listaResultado_.Item(6), Campo).Valor)

                    sc_TipoMoneda.DataSource.RemoveAll(Function(ch) ch.Value <> "")

                    Dim monedas_ = _icontroladorMonedas.BuscarMonedas(New List(Of String),
                                                                     New List(Of ObjectId) From {idMoneda}, "cveAcuseValor")

                    sc_TipoMoneda.DataSource = _organismo.
                                               ObtenerSelectOption(sc_TipoMoneda,
                                                                   monedas_.
                                                                   Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                            .Id = chi._id,
                                            .Valor = chi.nombremonedaesp & " | " &
                                            chi.aliasmoneda.Find(Function(ef) ef.Clave = "cveAcuseValor").Valor
                                           }).ToList)

                    If sc_TipoMoneda.DataSource.Count > 0 Then

                        sc_TipoMoneda.Value = idMoneda.ToString

                    End If

                    SetVars("_Monedas", monedas_)

                    If DirectCast(listaResultado_.Item(8), Campo).Valor.ToString = "2" Then

                        swc_Subdivision.Checked = True

                    Else

                        swc_Subdivision.Checked = False

                    End If

                    Dim identificacionPersona_, tipoIdentificador_ As String

                    If DirectCast(listaResultado_.Item(10), Campo).Valor IsNot Nothing Then

                        identificacionPersona_ = DirectCast(listaResultado_.Item(10), Campo).Valor.ToString

                        tipoIdentificador_ = "TAXID"

                    Else

                        If DirectCast(listaResultado_.Item(11), Campo).Valor IsNot Nothing Then
                            identificacionPersona_ = DirectCast(listaResultado_.Item(11), Campo).Valor.ToString

                            tipoIdentificador_ = "RFC"
                        Else

                            identificacionPersona_ = DirectCast(listaResultado_.Item(9), Campo).Valor.ToString

                            tipoIdentificador_ = "RAZONSOCIAL"

                            Dim iindiceaux_ As Int32 = identificacionPersona_.IndexOf("|")

                            If iindiceaux_ > 0 Then

                                identificacionPersona_ = identificacionPersona_.Substring(0, iindiceaux_ - 1)

                            Else

                                iindiceaux_ = identificacionPersona_.LastIndexOf("-")

                                If iindiceaux_ > 0 Then

                                    identificacionPersona_ = identificacionPersona_.Substring(0, iindiceaux_ - 1)

                                End If

                            End If

                        End If

                    End If

                    If identificacionPersona_ <> "" Then

                        Dim proveedorAcuseValor_ = _controladorProveedor.BuscarProveedor(identificacionPersona_, tipoIdentificador_)

                        If proveedorAcuseValor_ IsNot Nothing Then

                            fbc_Proveedor.Value = proveedorAcuseValor_.Id.ToString

                            fbc_Proveedor.Text = proveedorAcuseValor_.NombreCliente & " | " & proveedorAcuseValor_.FolioDocumento

                            If proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                Campo(CamposDomicilio.CA_DOMICILIO_FISCAL) IsNot Nothing Then

                                If proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                    Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion <> "" Then

                                    ic_DireccionProveedor.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                  Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                                Else

                                    ic_DireccionProveedor.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                  Campo(CamposDomicilio.CA_CALLE).Valor &
                                                                  " #" &
                                                                  proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                  Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor &
                                                                 " CP:  " &
                                                                 proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                 Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor &
                                                                 " " &
                                                                 proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                 Campo(CamposDomicilio.CA_COLONIA).Valor &
                                                                 " ," &
                                                                 proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                 Campo(CamposDomicilio.CA_PAIS).Valor

                                End If

                                If tipoIdentificador_ = "RAZONSOCIAL" Then

                                    If proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                        Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor IsNot Nothing Then

                                        ic_IDFiscalProveedor.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                     Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor

                                    Else
                                        ic_IDFiscalProveedor.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                     Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                                    End If

                                Else

                                    ic_IDFiscalProveedor.Value = identificacionPersona_

                                End If

                            End If

                        End If

                        SetVars("_ProveedorAcuseValor", proveedorAcuseValor_)

                    End If

                    If DirectCast(listaResultado_.Item(4), Campo).Valor IsNot Nothing Then

                        identificacionPersona_ = DirectCast(listaResultado_.Item(4), Campo).Valor.ToString

                        tipoIdentificador_ = "TAXID"

                    Else

                        If DirectCast(listaResultado_.Item(5), Campo).Valor IsNot Nothing Then

                            identificacionPersona_ = DirectCast(listaResultado_.Item(5), Campo).Valor.ToString

                            tipoIdentificador_ = "RFC"

                        Else


                            identificacionPersona_ = DirectCast(listaResultado_.Item(3), Campo).Valor.ToString

                            tipoIdentificador_ = "RAZONSOCIAL"

                            Dim iindiceAux_ As Int32 = identificacionPersona_.IndexOf("|")

                            If iindiceAux_ > 0 Then

                                identificacionPersona_ = identificacionPersona_.Substring(0, iindiceAux_ - 1)

                            Else

                                iindiceAux_ = identificacionPersona_.LastIndexOf("-")

                                If iindiceAux_ > 0 Then

                                    identificacionPersona_ = identificacionPersona_.Substring(0, iindiceAux_ - 1)

                                End If

                            End If

                        End If

                    End If

                    If identificacionPersona_ <> "" Then

                        Dim clienteAcuseValor_ As ConstructorCliente = _icontroladorEmpresa.BuscarCliente(identificacionPersona_,
                                                                                                         tipoIdentificador_)

                        With clienteAcuseValor_

                            If tipoIdentificador_ = "RAZONSOCIAL" Then

                                ic_IDFiscalDestinatario.Value = clienteAcuseValor_.Seccion(SeccionesClientes.SCS1).
                                                                                   Attribute(CamposClientes.CA_TAX_ID).Valor

                                If ic_IDFiscalDestinatario.Value = "" Then

                                    ic_IDFiscalDestinatario.Value = clienteAcuseValor_.Seccion(SeccionesClientes.SCS1).
                                                                                       Attribute(CamposClientes.CA_RFC_CLIENTE).Valor

                                End If

                            Else

                                ic_IDFiscalDestinatario.Value = identificacionPersona_

                            End If

                            fbc_Destinatario.Value = .Id

                            fbc_Destinatario.Text = .NombreCliente & " | " & .FolioDocumento

                            ic_DireccionDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                              Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                            sc_SelloCliente.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = clienteAcuseValor_.Id,
                                                                                                                .Text = ic_IDFiscalDestinatario.Value.ToString},
                                                                                         New SelectOption With {.Value = "0", .Text = "AGENTE ADUANAL"}}

                            sc_SelloCliente.Value = .Id

                            Dim clientetemporal_ = .Seccion(SeccionesClientes.SCS2).Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL)

                            Dim patenteaduanal_ As String

                            If clientetemporal_ Is Nothing Then

                                patenteaduanal_ = ""

                            Else

                                If clientetemporal_.Valor Is Nothing Then

                                    patenteaduanal_ = ""

                                Else

                                    patenteaduanal_ = clientetemporal_.Valor

                                End If

                            End If

                            If patenteaduanal_ <> "" Then

                                ic_PatenteAduanal.Value = .Seccion(SeccionesClientes.SCS2).
                                                          Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL).ValorPresentacion

                            Else

                                ic_PatenteAduanal.Value = "3921 | LUIS ENRIQUE DE LA CRUZ REYES"

                            End If

                        End With

                        SetVars("_ClienteAcuseValor", clienteAcuseValor_)

                    End If

                    Dim partidasFactura_ = listaResultado_.Item(12)

                    Dim pillboxControl_ As PillboxControl = DirectCast(pb_PartidasAcuseValor, PillboxControl)

                    pillboxControl_.ClearRows()

                    Dim unidadesT_ As New List(Of UnidadMedida)

                    Dim indice_ = 0

                    For Each partidaFactura_ In partidasFactura_.Nodos

                        pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)

                                                       Dim descripcionesAcuseValor_ As String = ""

                                                       Dim cantidadAcuseValor_ As String = ""

                                                       Dim precioUnitario_ As String = ""

                                                       Dim total_ As String = ""

                                                       Dim totalDolares_ As String = ""

                                                       Dim monedaFactura_ As String = ""

                                                       Dim marca_ As String = ""

                                                       Dim modelo_ As String = ""

                                                       Dim subModelo_ As String = ""

                                                       Dim numeroSerie_ As String = ""

                                                       Dim unidades_ As New List(Of UnidadMedida)

                                                       pillbox_.SetIndice(pillboxControl_.KeyField,
                                                                          indice_)

                                                       pillbox_.SetFiled(False)
                                                       ' If indice_ + 1 = 0 Then

                                                       If DirectCast(partidaFactura_.Nodos(12).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           descripcionesAcuseValor_ = DirectCast(partidaFactura_.
                                                                                                 Nodos(12).
                                                                                                 Nodos(0),
                                                                                                 Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(14).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           cantidadAcuseValor_ = DirectCast(partidaFactura_.
                                                                                            Nodos(14).
                                                                                            Nodos(0),
                                                                                            Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(8).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           precioUnitario_ = DirectCast(partidaFactura_.Nodos(8).Nodos(0), Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(4).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           total_ = DirectCast(partidaFactura_.Nodos(4).Nodos(0), Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(15).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           unidades_ = _controladorUnidadesMedida.BuscarUnidadesCOVE((DirectCast(partidaFactura_.Nodos(15).Nodos(0), Campo).ValorPresentacion),, 1)

                                                       Else

                                                           unidades_ = _controladorUnidadesMedida.BuscarUnidadesCOVE("PIEZA",, 1)

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(22).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           numeroSerie_ = DirectCast(partidaFactura_.Nodos(indice_ - 1).Nodos(22).Nodos(0), Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(23).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           marca_ = DirectCast(partidaFactura_.Nodos(23).Nodos(0), Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(24).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           modelo_ = DirectCast(partidaFactura_.Nodos(24).Nodos(0), Campo).Valor

                                                       End If

                                                       If DirectCast(partidaFactura_.Nodos(25).Nodos(0), Campo).Valor IsNot Nothing Then

                                                           subModelo_ = DirectCast(partidaFactura_.Nodos(25).Nodos(0), Campo).Valor

                                                       End If

                                                       sc_MonedaPrecioUnitarioPartida.DataSource = sc_TipoMoneda.DataSource

                                                       If sc_MonedaPrecioUnitarioPartida.DataSource.Count > 0 Then

                                                           sc_MonedaPrecioUnitarioPartida.Value = sc_TipoMoneda.Value

                                                           monedaFactura_ = sc_MonedaPrecioUnitarioPartida.Text

                                                       Else

                                                           sc_MonedaPrecioUnitarioPartida.DataSource = New List(Of SelectOption) From
                                                                                                          {New SelectOption With {.Value = "",
                                                                                                                                  .Text = ""}}
                                                       End If

                                                       If cantidadAcuseValor_ <> "" And precioUnitario_ <> "" Then

                                                           If total_ = "" Then

                                                               total_ = Convert.ToString(Convert.ToDouble(cantidadAcuseValor_) *
                                                                                         Convert.ToDouble(precioUnitario_))

                                                           End If

                                                           totalDolares_ = Convert.ToString(Convert.ToDouble(total_) *
                                                                                                _icontroladorMonedas.
                                                                                                Monedas.
                                                                                                Find(Function(se) se._id.ToString = sc_MonedaPrecioUnitarioPartida.Value.ToString).
                                                                                                factoresmoneda(0).valordefault)

                                                       End If

                                                       pillbox_.SetControlValue(ic_DescripcionAcuseValor, descripcionesAcuseValor_)

                                                       pillbox_.SetControlValue(ic_CantidadAcuseValor, cantidadAcuseValor_)

                                                       pillbox_.SetControlValue(ic_PrecioUnitarioAcuseValor, precioUnitario_)

                                                       pillbox_.SetControlValue(ic_ValorFacturaPartida, total_)

                                                       pillbox_.SetControlValue(ic_ValorDolaresPartida, totalDolares_)

                                                       pillbox_.SetControlValue(sc_MonedaPrecioUnitarioPartida,
                                                                                    New SelectOption With {.Value = sc_MonedaPrecioUnitarioPartida.Value,
                                                                                                           .Text = sc_MonedaPrecioUnitarioPartida.Text})

                                                       If unidades_.Count = 0 Then

                                                           unidades_ = _controladorUnidadesMedida.BuscarUnidadesCOVE("PIEZA", 1)

                                                       End If

                                                       unidades_ = unidades_.Union(unidadesT_).ToList

                                                       unidadesT_ = unidades_

                                                       Dim ltselecoption_ = New List(Of SelectOption)

                                                       For Each ltunidad_ In unidades_

                                                           ltselecoption_.Add(New SelectOption With {.Value = ltunidad_._id.ToString,
                                                                                                         .Text = ltunidad_.nombreoficialesp.ToUpper})

                                                       Next

                                                       sc_UnidadAcuseValor.DataSource = ltselecoption_

                                                       sc_UnidadAcuseValor.Value = unidades_(0)._id.ToString

                                                       pillbox_.SetControlValue(sc_UnidadAcuseValor,
                                                                                    New SelectOption With {.Value = sc_UnidadAcuseValor.Value,
                                                                                                           .Text = sc_UnidadAcuseValor.Text})

                                                       pillbox_.SetControlValue(ic_NumeroSerieAcuseValor, numeroSerie_)

                                                       pillbox_.SetControlValue(ic_MarcaAcuseValor, marca_)

                                                       pillbox_.SetControlValue(ic_ModeloAcuseValor, modelo_)

                                                       pillbox_.SetControlValue(ic_SubmodeloAcuseValor, subModelo_)

                                                   End Sub)

                        indice_ += 1
                    Next

                    pb_PartidasAcuseValor = pillboxControl_

                    pb_PartidasAcuseValor.PillBoxDataBinding()

                    Dim datos = New List(Of Dictionary(Of String, Object))

                    Dim identidad_ As Int32 = 1


                    pb_PartidasAcuseValor.DataSource.ToList().ForEach(Sub(c As Dictionary(Of String, Object))

                                                                          c.Item(pb_PartidasAcuseValor.KeyField) = 0

                                                                          c.Add("identidad", Str(identidad_))

                                                                          c.Add("estado", 1)

                                                                          datos.Add(c)

                                                                          identidad_ = identidad_ + 1

                                                                      End Sub)

                    pb_PartidasAcuseValor.DataSource = datos
                    ' SetVars("SetFill_", True)
                    ' Dim Smensajillo As String = "Factura  cargada " & dbc_NumFacturaAcuseValor.Value.ToString & " satisfactoriamente"
                    ' MsgBox()

                    MostrarFactor()

                    BloqueaObligatoriosFactura()

                    DisplayMessage("Factura  " &
                                   Chr(34) &
                                   dbc_NumFacturaAcuseValor.Value.ToString &
                                   Chr(34) &
                                   " cargada satisfactoriamente",
                                   StatusMessage.Info)

                Else

                    DisplayMessage("No se encontró un Documento con Folio " &
                                   Chr(34) &
                                   dbc_NumFacturaAcuseValor.Value.ToString &
                                   Chr(34),
                                   StatusMessage.Fail)

                End If

            Else

                DisplayMessage("Falta especificar el Folio del Documento",
                               StatusMessage.Fail)

            End If

        End If

    End Sub

    Public Sub scTipoDocumento_Click(sender As Object, e As EventArgs)

    End Sub

    Public Sub sc_TipoMoneda_Click(sender As Object, e As EventArgs)


        Dim monedas_ = _icontroladorMonedas.
                       BuscarMonedas(New List(Of String) From {"USD",
                                                               "EUR",
                                                               "MXP",
                                                               "CHF",
                                                               "JPY",
                                                               "CNY"},,
                                                               "cveAcuseValor")

        sender.dataSource = _organismo.ObtenerSelectOption(sc_TipoMoneda,
                                                           monedas_.
                                                           Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                                          .Id = chi._id,
                                                          .Valor = chi.nombremonedaesp &
                                                          " | " &
                                                          chi.aliasmoneda.Find(Function(ch) ch.Clave = "cveAcuseValor").Valor
                                                           }).ToList)

        SetVars("_Monedas", monedas_)
        'Dim Listilla As List(Of String) = _controladorMonedas.consultaM(_controladorMonedas.CamposBusquedaSimple.IDS,,)
        'Dim strincillo As String = ""
        'For Each elemento In Listilla
        '    strincillo = strincillo & elemento & Chr(13)
        'Next
        'MsgBox(strincillo)
    End Sub

    Public Sub sc_TipoMoneda_SelectedIndesxChanged(sender As Object, e As EventArgs)

        sc_MonedaPrecioUnitarioPartida.DataSource = sc_TipoMoneda.DataSource

        sc_MonedaPrecioUnitarioPartida.Value = sc_TipoMoneda.Value

        MostrarFactor()

    End Sub
    Public Sub sc_TipoMoneda_TextChanged(sender As Object, e As EventArgs)
        'If sc_TipoMoneda.Value <> "" Then
        '_controladorMonedas.ActualizaUltimaMoneda(New ObjectId(sc_TipoMoneda.Value))
        'End If

        Dim monedas_ = _icontroladorMonedas.BuscarMonedas(CType(sender.SuggestedText, String),,
                                                          "cveAcuseValor",
                                                          7)

        sender.dataSource = _organismo.ObtenerSelectOption(sender,
                                                           monedas_.
                                                           Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                                          .Id = chi._id,
                                                          .Valor = chi.nombremonedaesp &
                                                          " | " &
                                                          chi.aliasmoneda.Find(Function(ef) ef.Clave = "cveAcuseValor").Valor
                                                          }).ToList)
        'sender.Value = sender.datasource(0).Value
        SetVars("_Monedas", monedas_)

        ' sc_MonedaPrecioUnitarioPartida.DataSource = sc_TipoMoneda.DataSource
        ' sc_MonedaPrecioUnitarioPartida.Value = sc_TipoMoneda.Value
    End Sub



    Public Sub swc_CertificadoOrigen_CheckedChanged(sender As Object, e As EventArgs)

        ic_ExpotadorAutorizado.Visible = swc_CertificadoOrigen.Checked

    End Sub

    Protected Sub pb_PartidasAcuseValor_CheckedChange(sender As Object, e As EventArgs)

    End Sub

    Protected Sub pb_PartidasAcuseValor_Click(sender As Object, e As EventArgs)

        Select Case pb_PartidasAcuseValor.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo


                'OBTENEMOS LA MONEDA UTILIZADA EN LA PÁGINA ANTERIOR DEL PASTILLERO POR SI ES LA QUE VA USAR EL USUARIO
                Dim sc_ValorAnterior_ = pb_PartidasAcuseValor.DataSource(pb_PartidasAcuseValor.PageIndex - 2)

                Dim sValue_ As String = ""

                Dim sText_ As String = ""

                For Each sc_Valor In sc_ValorAnterior_

                    If sc_Valor.Key = "sc_MonedaPrecioUnitarioPartida" Then

                        Dim scadena_ = sc_Valor.ToJson.ToString

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Value"))

                        Dim indice_ = scadena_.IndexOf(":") + 3

                        Dim indicet_ = scadena_.IndexOf(",")

                        sValue_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Text"))

                        indice_ = scadena_.IndexOf(":") + 3

                        indicet_ = scadena_.IndexOf("}") - 1

                        sText_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                    End If

                Next

                sc_MonedaPrecioUnitarioPartida.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = sValue_,
                                                                                                                   .Text = sText_}}

                sc_MonedaPrecioUnitarioPartida.Value = sc_MonedaPrecioUnitarioPartida.DataSource(0).Value

                'OBTENEMOS LA UNIDAD DE MEDIDA UTILIZADA EN LA PÁGINA ANTERIOR DEL PASTILLERO POR SI ES LA QUE VA USAR EL USUARIO

                sValue_ = ""

                sText_ = ""

                For Each sc_Valor In sc_ValorAnterior_

                    If sc_Valor.Key = "sc_UnidadAcuseValor" Then

                        Dim scadena_ = sc_Valor.ToJson.ToString

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Value"))

                        Dim indice_ = scadena_.IndexOf(":") + 3

                        Dim indicet_ = scadena_.IndexOf(",")

                        sValue_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Text"))

                        indice_ = scadena_.IndexOf(":") + 3

                        indicet_ = scadena_.IndexOf("}") - 1

                        sText_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                    End If

                Next

                sc_UnidadAcuseValor.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = sValue_,
                                                                                                        .Text = sText_}}

                sc_UnidadAcuseValor.Value = sc_UnidadAcuseValor.DataSource(0).Value

                lbNumeroAcuseValor.Text = (pb_PartidasAcuseValor.PageIndex).ToString()

            Case PillboxControl.ToolbarActions.Borrar

            Case PillboxControl.ToolbarActions.Archivar

            Case Else

        End Select

    End Sub

    Protected Sub fbc_Proveedor_TextChanged(sender As Object, e As EventArgs)

        Dim lista_ = _controladorProveedor.BuscarProveedores1(sender.Text,
                                                              swc_TipoOperacion.Checked)

        sender.DataSource = lista_

    End Sub

    Protected Sub fbc_Proveedor_Click(sender As Object, e As EventArgs)

        If sender.Value.ToString <> "" Then
            'Aquí buscamos al proveedor y su dirección
            Select Case sender.ID

                Case "fbc_Proveedor"

                    Dim documentoProveedor_ = _controladorProveedor.BuscarProveedor(sender.Value.ToString,
                                                                                    "ID")

                    SetVars("_ProveedorAcuseValor", documentoProveedor_)

                    With documentoProveedor_

                        If .NombreCliente IsNot Nothing Then

                            ic_DireccionProveedor.Value = .Seccion(SeccionesProvedorOperativo.SPRO2).
                                                           Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                            If .Seccion(SeccionesProvedorOperativo.SPRO2).
                                Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor IsNot Nothing Then

                                If .Seccion(SeccionesProvedorOperativo.SPRO2).
                                    Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor = "" Then

                                    ic_IDFiscalProveedor.Value = .Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                  Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                                Else

                                    ic_IDFiscalProveedor.Value = .Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                 Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor

                                End If

                            Else

                                If .Seccion(SeccionesProvedorOperativo.SPRO2).
                                    Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor IsNot Nothing Then

                                    ic_IDFiscalProveedor.Value = .Seccion(SeccionesProvedorOperativo.SPRO2).
                                                                  Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                                End If

                            End If

                        End If

                    End With

                Case "fbc_Destinatario"

                    Dim documentoCliente_ = _icontroladorEmpresa.BuscarCliente(sender.Value.ToString, "ID")

                    SetVars("_ClienteAcuseValor", documentoCliente_)

                    With documentoCliente_

                        If .NombreCliente IsNot Nothing Then

                            ic_DireccionDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                              Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                            If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                                If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor = "" Then

                                    ic_IDFiscalDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                                     Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                                Else

                                    ic_IDFiscalDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                                     Campo(CamposClientes.CA_TAX_ID).Valor

                                End If

                            Else

                                If .Seccion(SeccionesClientes.SCS1).
                                    Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                                    ic_IDFiscalDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                                     Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                                End If

                            End If

                            sc_SelloCliente.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = documentoCliente_.Id.ToString, .Text = ic_IDFiscalDestinatario.Value.ToString}, New SelectOption With {.Value = "0", .Text = "AGENTE ADUANAL"}}

                            sc_SelloCliente.Value = documentoCliente_.Id.ToString

                            If .Seccion(SeccionesClientes.SCS2).
                                Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL).ValorPresentacion IsNot Nothing Then

                                ic_PatenteAduanal.Value = .Seccion(SeccionesClientes.SCS2).
                                Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL).Valor

                            Else

                                ic_PatenteAduanal.Value = "3921 | LUIS ENRIQUE DE LA CRUZ REYES"

                            End If

                        End If

                    End With

                Case Else

            End Select

        Else

            Select Case sender.ID

                Case "fbc_Proveedor"

                    ic_DireccionProveedor.Value = ""

                    ic_IDFiscalProveedor.Value = ""

                Case "fbc_Destinatario"

                    ic_DireccionDestinatario.Value = ""

                    ic_IDFiscalDestinatario.Value = ""

                Case Else

            End Select

        End If

    End Sub

    Protected Sub fbc_Destinatario_TextChanged(sender As Object, e As EventArgs)
        '  Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        ''  Dim lista_ As List(Of SelectOption) = controlador_.Buscar(sender.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        'sender.DataSource = lista_
        Dim lista_ = _icontroladorEmpresa.BuscarClientes1(sender.Text,
                                                          swc_TipoOperacion.Checked)

        sender.DataSource = lista_

    End Sub


    Protected Sub sc_UnidadAcuseValor_TextChanged(sender As Object, e As EventArgs)

        Dim listaUnidadesComponente_ As List(Of SelectOption)

        Dim listaUnidadMedida_ As List(Of UnidadMedida) = _controladorUnidadesMedida.BuscarUnidadesCOVE(sender.SuggestedText)

        If listaUnidadMedida_ Is Nothing Then

            listaUnidadesComponente_ = New List(Of SelectOption)

        Else

            listaUnidadesComponente_ = _organismo.ObtenerSelectOption(sc_UnidadAcuseValor,
                                                                      listaUnidadMedida_.
                                                                      Select(Function(chi) New ValorProvisionalOption With {
                                                                        .Id = chi._id,
                                                                        .Valor = chi.nombreoficialesp
                                                                      }).ToList)

        End If

        sender.Datasource = listaUnidadesComponente_

    End Sub




    Protected Sub sc_UnidadAcuseValor_Click(sender As Object, e As EventArgs)

        Dim listaunidades As List(Of SelectOption)

        listaunidades = _organismo.ObtenerSelectOption(sc_UnidadAcuseValor,
                                                       _controladorUnidadesMedida.
                                                       BuscarUnidadesCOVE(New List(Of String) From {"C62_1",
                                                                                                      "KGM",
                                                                                                      "CS",
                                                                                                      "SET",
                                                                                                      "C62_2",
                                                                                                      "KT",
                                                                                                      "TNE",
                                                                                                      "LM",
                                                                                                      "MIL",
                                                                                                      "MQ",
                                                                                                      "MTK",
                                                                                                      "BX",
                                                                                                      "LTR",
                                                                                                      "GRM"}).
                                                                                                      Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                                                                                          .Id = chi._id,
                                                                                                          .Valor = chi.nombreoficialesp
                                                                                                      }).ToList)

        sender.DataSource = listaunidades

    End Sub


    'Protected Sub sc_MonedaPrecioUnitarioPartida_TextChanged(sender As Object, e As EventArgs)
    '    ControladorMonedas.BuscarMonedas("cveCOVE", "esp", sc_MonedaPrecioUnitarioPartida.SuggestedText, GetVars("_FactoresMonedas"), 10)
    '    ControladorMonedas.ObtenerSelectOption(sc_MonedaPrecioUnitarioPartida.DataSource)
    '    SetVars("_FactoresMonedas", ControladorMonedas._factorMonedas)

    'End Sub

    Public Function Checallenado() As Boolean

        Dim bchecador_ As Boolean = True

        If fbc_Proveedor.Value.ToString = "" Then

            bchecador_ = False

        End If

        Return bchecador_

    End Function

    Public Sub LimpiarTodo()

        Dim listaSelectOption_ As List(Of SelectOption)

        dbc_NumFacturaAcuseValor.Value = ""

        dbc_NumFacturaAcuseValor.ValueDetail = ""

        swc_CertificadoOrigen.Value = False

        swc_TipoOperacion.Checked = True

        swc_RelacionFactura.Checked = False

        swc_Subdivision.Checked = False

        sc_TipoDocumento.Value = "15464654"

        icFechaExpedicion.Value = DateTime.UtcNow.Date.ToString("yyyy-MM-dd")

        Dim monedas_ As List(Of MonedaGlobal) = GetVars("_Monedas")


        monedas_ = _icontroladorMonedas.BuscarMonedas(New List(Of String) From {"USD",
                                                                                "EUR",
                                                                                "MXP",
                                                                                "CHF",
                                                                                "JPY",
                                                                                "CNY"},,
                                                                                "cveAcuseValor")

        sc_TipoMoneda.DataSource = _organismo.ObtenerSelectOption(sc_TipoMoneda,
                                                                  monedas_.
                                                                  Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                                                                 .Id = e._id,
                                                                 .Valor = e.nombremonedaesp &
                                                                  " | " &
                                                                  e.aliasmoneda.Find(Function(ef) ef.Clave = "cveAcuseValor").Valor
                                                                  }).ToList)

        sc_MonedaPrecioUnitarioPartida.DataSource = sc_TipoMoneda.DataSource

        sc_TipoMoneda.Value = sc_TipoMoneda.DataSource(0).Value

        sc_MonedaPrecioUnitarioPartida.Value = sc_TipoMoneda.DataSource(0).Value

        listaSelectOption_ = _organismo.ObtenerSelectOption(sc_UnidadAcuseValor, _controladorUnidadesMedida.BuscarUnidadesCOVE(New List(Of String) From {"C62_1", "KGM", "CS", "SET", "C62_2", "KT", "TNE", "LM", "MIL", "MQ", "MTK", "BX", "LTR", "GRM"}).Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                             .Id = e._id,
                            .Valor = e.nombreoficialesp
                           }).ToList)

        ic_Observaciones.Value = ""

        ic_ExpotadorAutorizado.Value = ""

        ic_DireccionProveedor.Value = ""

        fbc_Proveedor.Text = ""

        fbc_Proveedor.Value = ""

        ic_IDFiscalProveedor.Value = ""

        fbc_Destinatario.Text = ""

        fbc_Destinatario.Value = ""

        ic_IDFiscalDestinatario.Value = ""

        ic_DireccionDestinatario.Value = ""

        sc_UnidadAcuseValor.DataSource = listaSelectOption_

        sc_UnidadAcuseValor.Value = listaSelectOption_(0).Value

        ic_DescripcionAcuseValor.Value = ""

        ic_CantidadAcuseValor.Value = ""

        ic_PrecioUnitarioAcuseValor.Value = ""

        ic_ValorFacturaPartida.Value = ""

        ic_ValorDolaresPartida.Value = ""

        ic_MarcaAcuseValor.Value = ""

        ic_ModeloAcuseValor.Value = ""

        ic_SubmodeloAcuseValor.Value = ""

        ic_NumeroSerieAcuseValor.Value = ""

        ic_PatenteAduanal.Value = ""

        sc_SelloCliente.DataSource = New List(Of SelectOption)

        DesbloqueaObligatoriosFactura()

    End Sub

    Public Sub MostrarFactor()

        Dim monedas_ As List(Of MonedaGlobal) = GetVars("_Monedas")

        If monedas_ IsNot Nothing Then

            Dim moneda_ = monedas_.Find(Function(ch) ch._id.ToString = sc_TipoMoneda.Value.ToString)

            '  MsgBox("Factor: $" & Moneda_.factoresmoneda(0).valordefault & " al " & Moneda_.factoresmoneda(0).fecha.ToString("dd-MM-yyyy"))

            sc_TipoMoneda.ToolTip = "Factor: $" &
                                     moneda_.factoresmoneda(0).valordefault &
                                     " al " &
                                     moneda_.factoresmoneda(0).fecha.ToString("dd-MM-yyyy")
            'DisplayMessage("Factor: $" & Monedilla_.factoresmoneda(0).valordefault & " al " & Monedilla_.factoresmoneda(0).fecha.ToString("dd-MM-yyyy"), StatusMessage.Info)
            'MsgBox("Factor: $" & factormonedas_(sc_TipoMoneda.Value.ToString).valorfactor & " al " & factormonedas_(sc_TipoMoneda.Value.ToString).Fecha.ToString("dd-MM-yyyy"), StatusMessage.Info)
        End If

    End Sub

    Public Sub BloqueaObligatoriosFactura()


        dbc_NumFacturaAcuseValor.Enabled = False

        swc_TipoOperacion.Enabled = False

        sc_TipoDocumento.Enabled = False

        sc_TipoMoneda.Enabled = False

        icFechaExpedicion.Enabled = False

        swc_Subdivision.Enabled = False

        fbc_Proveedor.Enabled = False

        ic_DireccionProveedor.Enabled = False

        ic_IDFiscalProveedor.Enabled = False

        fbc_Destinatario.Enabled = False

        ic_IDFiscalDestinatario.Enabled = False

        ic_DireccionDestinatario.Enabled = False

        sc_MonedaPrecioUnitarioPartida.Enabled = False

        ic_ValorFacturaPartida.Enabled = False

        sc_SelloCliente.Enabled = False

        ic_PatenteAduanal.Enabled = False

    End Sub

    Public Sub DesbloqueaObligatoriosFactura()


        dbc_NumFacturaAcuseValor.Enabled = True

        swc_TipoOperacion.Enabled = True

        sc_TipoDocumento.Enabled = True

        sc_TipoMoneda.Enabled = True

        icFechaExpedicion.Enabled = True

        swc_Subdivision.Enabled = True

        fbc_Proveedor.Enabled = True

        ic_DireccionProveedor.Enabled = True

        ic_IDFiscalProveedor.Enabled = True

        fbc_Destinatario.Enabled = True

        ic_IDFiscalDestinatario.Enabled = True

        ic_DireccionDestinatario.Enabled = True

        sc_MonedaPrecioUnitarioPartida.Enabled = True

        ic_ValorFacturaPartida.Enabled = True

        sc_SelloCliente.Enabled = True

        ic_PatenteAduanal.Enabled = True

    End Sub

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██ Defina en esta región todo lo que involucre el uso de controladores externos al contexto actual██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


#End Region


End Class
