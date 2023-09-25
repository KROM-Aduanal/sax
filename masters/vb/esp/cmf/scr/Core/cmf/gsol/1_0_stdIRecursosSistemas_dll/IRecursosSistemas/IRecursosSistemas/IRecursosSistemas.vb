Imports System.Runtime.Serialization
'Imports System.ServiceModel
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization

Public Interface IRecursosSistemas

#Region "Enums"

    Enum Tokens
        DigitalizacionMaestroDocumentosKBWeb = 1
        DigitalizacionMaestroDocumentosKBWebLogistica = 2
    End Enum

    Enum RecursosCMF

        <EnumMember> <Description("Undef")> SinDefinir = 0
        <EnumMember> <Description("CapaN")> FormularioSencillo64 = 1
        <EnumMember> <Description("CapaN")> GeneraDocumento64 = 2
        <EnumMember> <Description("CapaN")> GesMetas64 = 3
        <EnumMember> <Description("CapaN")> GestionContabilidadElectronica64 = 4
        <EnumMember> <Description("CapaN")> GestionDocumentosPagosHechos64 = 5
        <EnumMember> <Description("CapaN")> GestionGruposImpuestos = 6
        <EnumMember> <Description("CapaN")> GestionMaestroOperaciones64 = 7
        <EnumMember> <Description("CapaN")> GestionMetas64 = 8
        <EnumMember> <Description("CapaN")> GestionVinPermisos64 = 9
        <EnumMember> <Description("CapaN")> GestRefFreightForwarder64 = 10
        <EnumMember> <Description("CapaN")> GsDialogo64 = 11
        <EnumMember> <Description("CapaN")> gso_krom_DimensionesKrom64 = 12
        <EnumMember> <Description("CapaN")> gso_krom_IDimensionesKrom = 13
        <EnumMember> <Description("CapaN")> gso_krom_IReferencia = 14
        <EnumMember> <Description("CapaN")> gso_krom_PagosTerceros64 = 15
        <EnumMember> <Description("CapaN")> Gso_Nucleo_Modulos_GestionContenedores64 = 16
        <EnumMember> <Description("CapaN")> Gsol_Accion32 = 17
        <EnumMember> <Description("CapaN")> Gsol_AtributosEntidad64 = 18
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_IPoliticasBaseDatos = 19
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_Operaciones_CaracteristicaCatlogo64 = 20
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_Operaciones_EnvironmentViews64 = 21
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_Operaciones_ICaracteristicas = 22
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_Operaciones_IOperacionesCatalogo = 23
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64 = 24
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_Operaciones_OperacionesCatalogo64 = 25
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_PoliticasBaseDatos64 = 26
        <EnumMember> <Description("CapaN")> Gsol_BaseDatos_SysExpert64 = 27
        <EnumMember> <Description("CapaN")> gsol_basededatos_ConexionesNoSQL64 = 28
        <EnumMember> <Description("CapaN")> gsol_basededatos_IConexionesNoSQL = 29
        <EnumMember> <Description("CapaN")> Gsol_Clientes_Cat016MonitorContenedores = 30
        <EnumMember> <Description("CapaN")> Gsol_Clientes_Cat050OperacionesDiariosWalmart64 = 31
        <EnumMember> <Description("CapaN")> Gsol_Clientes_Ges016MonitorContenedores = 32
        <EnumMember> <Description("CapaN")> Gsol_Clientes_Ges050CargaAsignaciones64 = 33
        <EnumMember> <Description("CapaN")> Gsol_Clientes_Ges050OperacioneesDiariasWalmart64 = 34
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GesAddendaContinentalTire64 = 35
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GesAddendaGroupeSeb64 = 36
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GesAddendaPancosma64 = 37
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GesCapturaContinental64 = 38
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GesDetalleAddendaContinentalTire = 39
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GesDetalleAddendaLorealSLP64 = 40
        <EnumMember> <Description("CapaN")> Gsol_Clientes_GestorAddendaLorealSLP64 = 41
        <EnumMember> <Description("CapaN")> Gsol_Componente32 = 42
        <EnumMember> <Description("CapaN")> gsol_Componentes_DlgProgressBar = 43
        <EnumMember> <Description("CapaN")> gsol_Componentes_DynamicDocumentCharacteristic64 = 44
        <EnumMember> <Description("CapaN")> gsol_Componentes_LockButton64 = 45
        <EnumMember> <Description("CapaN")> Gsol_ConstructorVisual64 = 46
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_CatalogoMetodosPago64 = 47
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_ConceptoTransaccionPoliza64 = 48
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_DetallePolizas64 = 49
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_GestionConceptoTransaccionPoliza64 = 50
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_GestionMetodosPago64 = 51
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_GestionPolizas64 = 52
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_IContabilidad = 53
        <EnumMember> <Description("CapaN")> gsol_Contabilidad_ImprimePoliza64 = 54
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_PagosHechos64 = 55
        <EnumMember> <Description("CapaN")> Gsol_Contabilidad_Polizas64 = 56
        <EnumMember> <Description("CapaN")> Gsol_Controladores_Contabilidad64 = 57
        <EnumMember> <Description("CapaN")> Gsol_CorreoElectronico_IOperacionesCorreoElectronico = 58
        <EnumMember> <Description("CapaN")> Gsol_CorreoElectronico_OperacionesCorreoElectronico64 = 59
        <EnumMember> <Description("CapaN")> Gsol_Credenciales64 = 60
        <EnumMember> <Description("CapaN")> Gsol_Data_Enum64 = 61
        <EnumMember> <Description("CapaN")> gsol_documento_Documentos64 = 62
        <EnumMember> <Description("CapaN")> gsol_documento_generaDocumento = 63
        <EnumMember> <Description("CapaN")> Gsol_Documento_GeneraDocumentoXML = 64
        <EnumMember> <Description("CapaN")> Gsol_Documento_GeneraDocumentoXMLCE = 65
        <EnumMember> <Description("CapaN")> gsol_documento_IDocumento = 66
        <EnumMember> <Description("CapaN")> gsol_documento_IgeneraDocumento = 67
        <EnumMember> <Description("CapaN")> Gsol_Documento_IOperacionesDocumento = 68
        <EnumMember> <Description("CapaN")> Gsol_Documento_IOperacionesDocumentoExtranet = 69
        <EnumMember> <Description("CapaN")> Gsol_Documento_LeerDocumento64 = 70
        <EnumMember> <Description("CapaN")> Gsol_Documento_OperacionesDocumento64 = 71
        <EnumMember> <Description("CapaN")> Gsol_Documento_OperacionesDocumentoExtranet64 = 72
        <EnumMember> <Description("CapaN")> Gsol_Documento_OperacionesDocumentoExtranetAntigua64 = 73
        <EnumMember> <Description("CapaN")> Gsol_EnsambladorCodigo64 = 74
        <EnumMember> <Description("CapaN")> Gsol_Entidad32 = 75
        <EnumMember> <Description("CapaN")> Gsol_EspacioTrabajo32 = 76
        <EnumMember> <Description("CapaN")> Gsol_EstructuraDatos_IEstructuraDatos = 77
        <EnumMember> <Description("CapaN")> Gsol_EventoEntidades64 = 78
        <EnumMember> <Description("CapaN")> Gsol_FormluarioBaseMaestroDetalle64 = 79
        <EnumMember> <Description("CapaN")> Gsol_FormularioBase64 = 80
        <EnumMember> <Description("CapaN")> Gsol_FormularioBaseCatalogo64 = 81
        <EnumMember> <Description("CapaN")> Gsol_Funcion32 = 82
        <EnumMember> <Description("CapaN")> Gsol_IAtributo = 83
        <EnumMember> <Description("CapaN")> Gsol_IConstructorVisual = 84
        <EnumMember> <Description("CapaN")> Gsol_ICredenciales = 85
        <EnumMember> <Description("CapaN")> Gsol_IEnsambladorCodigo = 86
        <EnumMember> <Description("CapaN")> Gsol_IEntidad = 87
        <EnumMember> <Description("CapaN")> Gsol_IEspacioTrabajo = 88
        <EnumMember> <Description("CapaN")> Gsol_IEstructuraDatos = 89
        <EnumMember> <Description("CapaN")> Gsol_IEvento = 90
        <EnumMember> <Description("CapaN")> Gsol_IIniciaSesion = 91
        <EnumMember> <Description("CapaN")> Gsol_ILeerArchivo = 92
        <EnumMember> <Description("CapaN")> Gsol_IniciaSesion64 = 93
        <EnumMember> <Description("CapaN")> Gsol_IPermisos = 94
        <EnumMember> <Description("CapaN")> gsol_IRecursosSistemas = 95
        <EnumMember> <Description("CapaN")> gsol_IRecursosSistemas64 = 97
        <EnumMember> <Description("CapaN")> Gsol_ISectorEntorno = 98
        <EnumMember> <Description("CapaN")> Gsol_ISesion = 99
        <EnumMember> <Description("CapaN")> Gsol_ISesionWcf = 100
        <EnumMember> <Description("CapaN")> gsol_krom_AdaptadorDatos64 = 101
        <EnumMember> <Description("CapaN")> gsol_krom_CampoVirtual64 = 102
        <EnumMember> <Description("CapaN")> gsol_krom_CatalogoWeb64 = 103
        <EnumMember> <Description("CapaN")> gsol_krom_CatResponsables64 = 104
        <EnumMember> <Description("CapaN")> gsol_krom_Causales64 = 105
        <EnumMember> <Description("CapaN")> gsol_krom_ConfiguracionBaseDeDatos64 = 106
        <EnumMember> <Description("CapaN")> gsol_krom_ControladorPeticiones64 = 107
        <EnumMember> <Description("CapaN")> gsol_krom_ControladorWeb64 = 108
        <EnumMember> <Description("CapaN")> gsol_krom_CopiasSimples64 = 109
        <EnumMember> <Description("CapaN")> gsol_krom_CuentaGastos64 = 110
        <EnumMember> <Description("CapaN")> gsol_krom_EnlaceDatos64 = 111
        <EnumMember> <Description("CapaN")> gsol_krom_EntidadDatos64 = 112
        <EnumMember> <Description("CapaN")> gsol_krom_Facturas64 = 113
        <EnumMember> <Description("CapaN")> gsol_krom_FormulariosFeedBack64 = 114
        <EnumMember> <Description("CapaN")> gsol_krom_Fracciones64 = 115
        <EnumMember> <Description("CapaN")> gsol_krom_GeneradorLinks64 = 116
        <EnumMember> <Description("CapaN")> gsol_krom_IAdaptadorDatos64 = 117
        <EnumMember> <Description("CapaN")> gsol_krom_IControladorPeticiones64 = 118
        <EnumMember> <Description("CapaN")> gsol_krom_IEnlaceDatos = 119
        <EnumMember> <Description("CapaN")> gsol_krom_IEntidadDatos = 120
        <EnumMember> <Description("CapaN")> gsol_krom_IGeneradorLinks = 121
        <EnumMember> <Description("CapaN")> gsol_krom_IPagosTerceros = 122
        <EnumMember> <Description("CapaN")> gsol_krom_LDAHidrocarburos64 = 123
        <EnumMember> <Description("CapaN")> gsol_krom_LineaBaseEnlaceDatos64 = 124
        <EnumMember> <Description("CapaN")> gsol_krom_MenuDinamico64 = 125
        <EnumMember> <Description("CapaN")> gsol_krom_Mercancias64 = 126
        <EnumMember> <Description("CapaN")> gsol_krom_Referencia64 = 127
        <EnumMember> <Description("CapaN")> gsol_krom_RegistroCausales64 = 128
        <EnumMember> <Description("CapaN")> gsol_krom_ReglasAccesoKrom64 = 129
        <EnumMember> <Description("CapaN")> gsol_krom_Responsables64 = 130
        <EnumMember> <Description("CapaN")> gsol_krom_Seguimiento64 = 131
        <EnumMember> <Description("CapaN")> gsol_krom_web_GeneradorLinks64 = 132
        <EnumMember> <Description("CapaN")> gsol_krom_web_IGeneradorLinks = 133
        <EnumMember> <Description("CapaN")> Gsol_LeerArchivoXML32 = 134
        <EnumMember> <Description("CapaN")> Gsol_LineaBaseIniciaSesion64 = 135
        <EnumMember> <Description("CapaN")> gsol_Modules_GesEstadoOperacion64 = 136
        <EnumMember> <Description("CapaN")> Gsol_Modulos_CatalogoMaestroDocumentos64 = 137
        <EnumMember> <Description("CapaN")> Gsol_Modulos_CatalogoSeguimiento64 = 138
        <EnumMember> <Description("CapaN")> Gsol_Modulos_DatawareHouse64 = 139
        <EnumMember> <Description("CapaN")> Gsol_Modulos_DatawareHouse6445 = 140
        <EnumMember> <Description("CapaN")> Gsol_Modulos_DetalleMovimientosMasivo64 = 141
        <EnumMember> <Description("CapaN")> Gsol_modulos_frm007GestorSeguimiento64 = 142
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GenericCatalog64 = 143
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GenericOperators64 = 144
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GesMaestroOperaciones64 = 145
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestionAsociadorDocumentos = 146
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestionTiposPlantillasDocumentos64 = 147
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestionUnileverHV64 = 148
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorAccesoUsuariosKromApp = 149
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorClasificadorDocumentos64 = 150
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorConfiguracionCamposRequeridos64 = 151
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorDetLDA64 = 152
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorDetLDAHidrocarburos64 = 153
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorEncLDA64 = 154
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorEncLDAHidrocarburos64 = 155
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorEncTracking64 = 156
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorInformacionRevalidacion64 = 157
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorMaestroDocumentos64 = 158
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorPreReferencia64 = 159
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorUsuariosSysExpert64 = 160
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorVinMaestroOperacionesTracking64 = 162
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestorVinMaestroOT64 = 163
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GestRefFreightForwarder64 = 164
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GruposClientesEjecutivos64 = 165
        <EnumMember> <Description("CapaN")> Gsol_Modulos_GruposUsuarios64 = 166
        <EnumMember> <Description("CapaN")> Gsol_Modulos_Hitos64 = 167
        <EnumMember> <Description("CapaN")> Gsol_Modulos_MaestroOperacionesOtrosDatos64 = 168
        <EnumMember> <Description("CapaN")> Gsol_Modulos_PeriodosContables64 = 169
        <EnumMember> <Description("CapaN")> Gsol_Modulos_PlantillaFlujo64 = 170
        <EnumMember> <Description("CapaN")> Gsol_Modulos_PoliticasCorporativas64 = 171
        <EnumMember> <Description("CapaN")> Gsol_Modulos_PreReferencia64 = 172
        <EnumMember> <Description("CapaN")> Gsol_Modulos_Pruebas64 = 173
        <EnumMember> <Description("CapaN")> Gsol_Modulos_Referencia64 = 174
        <EnumMember> <Description("CapaN")> Gsol_Modulos_RelacionContenedoresImpo64 = 175
        <EnumMember> <Description("CapaN")> Gsol_Modulos_RevalidacionImpo64 = 176
        <EnumMember> <Description("CapaN")> Gsol_Modulos_TiposPoliticasCorporativas64 = 177
        <EnumMember> <Description("CapaN")> Gsol_Modulos_TiposSeguimiento64 = 178
        <EnumMember> <Description("CapaN")> Gsol_Modulos_VinAccionesOperacionales64 = 179
        <EnumMember> <Description("CapaN")> Gsol_Modulos_VinCamposPlantillas64 = 180
        <EnumMember> <Description("CapaN")> Gsol_Modulos_VisorArchivoMaestroDocumentos64 = 181
        <EnumMember> <Description("CapaN")> Gsol_Modulos_VisorAsociadorMaestroDocumentos64 = 182
        <EnumMember> <Description("CapaN")> Gsol_Modulos_VisorMaestroDocumentos64 = 183
        <EnumMember> <Description("CapaN")> Gsol_Monitoreo_BitacoraCapaDatos64 = 184
        <EnumMember> <Description("CapaN")> Gsol_Monitoreo_LineaBaseBitacora64 = 185
        <EnumMember> <Description("CapaN")> gsol_monitoreo_SingletonBitacoras64 = 186
        <EnumMember> <Description("CapaN")> Gsol_Namespace_IEstructuraDatos = 187
        <EnumMember> <Description("CapaN")> gsol_notificaciones_INotificaciones = 188
        <EnumMember> <Description("CapaN")> gsol_notificaciones_Notificaciones64 = 189
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_FacturacionCorresponsalias64 = 190
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_FacturaObjeto3364 = 191
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_FacturaObjeto64 = 193
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulo_GestionAuditoriasPoliticas64 = 194
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Acciones64 = 195
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AgenciasAduanales64 = 196
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AgentesAduanales64 = 197
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Anticipos64 = 198
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Aplicaciones64 = 199
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AplicacionesModulos64 = 200
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AuditoriaCalidad64 = 201
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AuditoriaJuridico64 = 202
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AuditoriaPago64 = 203
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_AuxiliaresContables64 = 204
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_BalanzaDeComprobacion64 = 205
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Bancos64 = 206
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Beneficiarios64 = 207
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_BillOfLading64 = 208
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_BillOfLadingDomain64 = 209
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_BitacoraAdministrador64 = 210
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CapturaMovimientos64 = 211
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CaracEquipoComputo64 = 212
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CaracteristicasCotizacion64 = 213
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Cartera64 = 214
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CatalogoMiCarteraClientes = 215
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CatalogoReportes64 = 216
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CategoriasCuentaContable64 = 217
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Causales64 = 218
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ChequePoliza64 = 219
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Ciudades64 = 220
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ClasificacionConceptosPagos64 = 221
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Clientes64 = 222
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ClientesComercializacion64 = 223
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Comercializacion64 = 224
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Componentes64 = 225
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Comportamiento64 = 226
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ConceptosPago64 = 227
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ConceptosPagosHechos64 = 228
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ConceptosPagosOficinas64 = 229
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ContabilidadAutoridad64 = 230
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Contabilidadelectronica64 = 231
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Contactos64 = 232
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ContolCFDI64 = 233
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ControlVersiones64 = 234
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CostosInsumos64 = 235
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CostosRiesgos64 = 236
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Cotizaciones64 = 237
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CtasBancariasEntidad64 = 238
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CuentaCliente64 = 239
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CuentasBancariasBeneficiarios64 = 240
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CuentasBancariasMisBancos64 = 241
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_CuentasContables64 = 242
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Departamentos64 = 243
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleAnticipos64 = 244
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleContenedoresBL64 = 245
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleDocumentos64 = 246
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleEjemplos64 = 247
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleEmisionPagos64 = 248
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleEmisionPagos64Beta = 249
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleMercanciasBL64 = 250
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleMovimientos64 = 251
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleNotificacionesBL64 = 252
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleOtrosCargos64 = 253
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetallePagosHechos64 = 254
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleTarifaPricingCarac64 = 255
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleTarifaPricingInfo64 = 256
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DetalleTransaccionesPago64 = 257
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DivisionesEmp64 = 258
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_DivisionOperativa64 = 259
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Domicilios64 = 260
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_EjecutivosGruposTrabajo64 = 261
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_EjecutivosMiEmpresa64 = 262
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_EmpresasGeneral64 = 263
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Entidades64 = 264
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_EquiposComputo64 = 265
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_EstadosPaises64 = 266
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_EtiquetarDivision64 = 267
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_FasesComercializacion64 = 268
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_FormasPago64 = 269
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Funciones64 = 270
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GeneradorReportesContabilidad64 = 271
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesCarCotizaciones64 = 272
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesConfigContabilidad64 = 273
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesControlVersiones64 = 274
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesDetallesCotizacion64 = 275
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesDetKardexDivision64 = 276
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesEtapasComercializacion64 = 277
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesFasesComercializacion64 = 278
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesKardexEquiEjec64 = 279
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesMetas64 = 280
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesMovComercializacion64 = 282
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesReferenciasVistosBuenos64 = 283
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesRegPer64 = 284
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesTabuladorComisiones64 = 285
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestCaracEquipos64 = 286
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestComprobantesExts64 = 287
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestConcCtasPorPagar64 = 288
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestConcCuentasxPag64 = 289
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAgenciasAduanales64 = 290
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAgentesCarga64 = 291
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAplicacinesModulos64 = 292
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAplicaciones64 = 293
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAtributos64 = 294
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAuditoriasOficinas64 = 295
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAuditoriasPoliticas64 = 296
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionAuditoriasPrevias64 = 297
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionBancos = 298
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionBancos64 = 299
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionBancosSAT64 = 300
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionBeneficiarios64 = 301
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCaracEquipoComputo64 = 302
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCaracTarifaPricing64 = 303
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCaractCotiza64 = 304
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCaractEquipo64 = 305
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCaracteristicasComer64 = 306
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCartera64 = 307
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCatalogoAuditorias64 = 308
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCategoriasCuentaContable64 = 309
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCausales64 = 310
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionChequePoliza64 = 311
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCiudades64 = 312
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionClientes64 = 313
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionComponentes64 = 314
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionConceptosFacturacion64 = 315
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionConceptosPagos64 = 316
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionConceptosPagosHechos64 = 317
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionConfiguracionAuditorias64 = 318
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionContactos64 = 319
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionContElectronica64 = 320
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionControlCFDI64 = 321
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCotizaciones64 = 322
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCtasBancariasEnt64 = 323
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCuentasBancarias64 = 324
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCuentasBancariasBeneficiarios64 = 325
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCuentasBeneficiarios64 = 326
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCuentasContables64 = 327
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCuentasMisBancos64 = 328
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionCuentasPorPagar64 = 329
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDepartamentos64 = 330
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDetalleChequePliza = 331
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDetalleKardex64 = 332
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDetalleMovimientos64 = 333
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDetCotizacion64 = 334
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDivEmp64 = 335
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDocumentosCDFI = 336
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionDomicilios64 = 337
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEjecutivosMiEmpresa64 = 338
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEjemplos64 = 339
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEmisionPagos64 = 340
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEmisionPagos64Beta = 341
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEmpresasGeneral64 = 342
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEntidades64 = 343
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEquipos64 = 344
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEstatusCuentasPorPagar64 = 345
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEstatusReferencias64 = 346
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionEstPai64 = 347
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionFactorMoneda64 = 348
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionFacturasSolicPago64 = 349
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionFormasPago = 350
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionFormatosArchivos64 = 351
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionFunciones64 = 352
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionGirosEmpresariales64 = 353
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionGruposImpuestos64 = 354
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionGruposVenta64 = 355
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionHouseBL64 = 356
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionImpuestos64 = 357
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionKardex64 = 358
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionKardexDivisiones64 = 359
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionListaValores64 = 360
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionMaestroOperaciones64 = 361
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionMetas64 = 362
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionMisBancos64 = 363
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionMisEmpresas64 = 364
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionModulos64 = 365
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionMonitorSolicitud64 = 366
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionNaves64 = 367
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionNavieras64 = 368
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionNegocioPotencial64 = 369
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionNivelesTabulador64 = 370
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionNotasCotizaciones64 = 371
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPagosHechos64 = 372
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPaises64 = 373
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPerfiles64 = 374
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPermisos64 = 375
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPersonas64 = 376
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPlantillasTracking64 = 377
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPolizaCierre64 = 378
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionProductosServicios64 = 379
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionProspectoEjecutivo64 = 380
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionProspectos64 = 381
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionProveedores64 = 382
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionPuestos64 = 383
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionRecuperarComprobantes64 = 384
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionRegistroPerfiles64 = 385
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionReportes64 = 386
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionResponsables64 = 387
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionRoles64 = 388
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionRolesPerfiles64 = 389
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionServidores64 = 390
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionSolicitudCheque = 391
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionSolicitudPago64 = 392
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionStatusObjetos64 = 393
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTarifaPricing64 = 394
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTarifario64 = 395
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTipoCambio64 = 396
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTiposAuditorias64 = 397
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTiposCotizacion64 = 398
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTiposDescuentos64 = 399
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTiposDocumentos64 = 400
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTiposMoneda64 = 401
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionTracking64 = 402
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionUnidadesMedida64 = 403
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionUnidadesNegocio64 = 404
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionUsuarios64 = 405
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVinculoAcciones64 = 406
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVinculoDetallesTracking64 = 407
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVinculoImpuestos64 = 408
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVinPerfiles64 = 409
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVinPermisos64 = 410
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVinRolesPermisos64 = 411
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestionVistoBueno64 = 412
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestorClasificadorDocumentos64 = 413
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestorDetUsuariosCuentaCliente64 = 414
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestorPolizaSeguro64 = 415
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestorRecordatorios = 416
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestorUnidadesNegociadas64 = 417
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestorUsuariosCuentaCliente64 = 418
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestReferenciasOperativas64 = 419
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestRefFreightForwarder64 = 420
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestReposDigitales64 = 421
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GestTokensContabilidad64 = 422
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GesVinProductosServicios64 = 423
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GirosEmpresariales64 = 424
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GruposEmpresariales64 = 425
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GruposImpuestos64 = 426
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_GruposVenta64 = 427
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Hitos64 = 428
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Idiomas64 = 429
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Impuestos64 = 430
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Incoterms64 = 431
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Insumos64 = 432
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Kardex64 = 433
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_KardexDivisionesEmpresariales64 = 434
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_KardexEquiposEjecutivos64 = 435
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Metas64 = 436
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_MisEmpresas64 = 437
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Modulos64 = 438
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Monedas64 = 439
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_MotivosSubDivisiones64 = 440
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_MovComercializacion64 = 441
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Naves64 = 442
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Navieras64 = 443
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_NegocioPotencial64 = 444
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_NotasCotizacion64 = 445
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_PagosHechos64 = 446
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Paises64 = 447
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_PerfilEjecutivo64 = 448
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_PerfilEjecutivoCliente64 = 449
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Perfiles64 = 450
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Permisos64 = 451
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Personas64 = 452
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_PolizasContables64 = 453
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_PrevioComprobantesExt64 = 454
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Procesos64 = 455
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ProductosOperativos64 = 456
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ProductosServicios64 = 457
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Prospectos64 = 458
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Proveedores64 = 459
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Puertos64 = 460
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Puestos64 = 461
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Recodatorios64 = 462
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_RegistroPerfiles64 = 463
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ReglasCuentaCliente64 = 464
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_ReportesComercializacion64 = 465
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Responsables64 = 466
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Riesgos64 = 467
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Roles64 = 468
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_SolicitudCuentasBancarias64 = 469
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_SolicitudesCheque64 = 470
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_SolicitudGestionConceptosPagosHechos64 = 471
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_SolicitudPago64 = 472
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_StatusObjetos64 = 473
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_StatusProspectos64 = 474
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TabuladorComisiones64 = 475
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Tarifario64 = 476
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TarifasPricing64 = 477
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TipoCambio64 = 478
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TipoCartera64 = 479
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TipoCuenta64 = 480
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TipoDesarrollo64 = 481
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TipoPerfilEjecutivo64 = 482
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TiposCotizacion64 = 483
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TiposDescuentos64 = 484
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TiposDocumento64 = 485
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TiposDocumentoAdministrativo64 = 486
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_TiposMoneda64 = 487
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Tracking64 = 488
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_UnidadesMedida64 = 489
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_UnidadesNegociadas64 = 490
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_Usuarios64 = 491
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_VinPermisos64 = 492
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_VinProductosComer64 = 493
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Modulos_VinProductosServicios64 = 494
        <EnumMember> <Description("CapaN")> Gsol_Nucleo_Procesos_Addendas64 = 495
        <EnumMember> <Description("CapaN")> Gsol_OperacionesCorreoElectronico64 = 496
        <EnumMember> <Description("CapaN")> Gsol_Organismo64 = 497
        <EnumMember> <Description("CapaN")> Gsol_Permiso32 = 498
        <EnumMember> <Description("CapaN")> gsol_Procesos_Controladores_Auditorias64 = 499
        <EnumMember> <Description("CapaN")> gsol_procesos_corporativo_IAuditoriasVistosBuenos = 500
        <EnumMember> <Description("CapaN")> Gsol_SectorEntorno32 = 501
        <EnumMember> <Description("CapaN")> Gsol_Secuencias64 = 502
        <EnumMember> <Description("CapaN")> Gsol_Seguridad_Cifrado25664 = 503
        <EnumMember> <Description("CapaN")> gsol_seguridad_Configuracion64 = 504
        <EnumMember> <Description("CapaN")> Gsol_Seguridad_ICifrado = 505
        <EnumMember> <Description("CapaN")> GSol_Service_Tracking64 = 506
        <EnumMember> <Description("CapaN")> Gsol_Sesion64 = 507
        <EnumMember> <Description("CapaN")> GSol_SesionWcf64 = 508
        <EnumMember> <Description("CapaN")> Gsol_Tools_EthernetInterfaces64 = 509
        <EnumMember> <Description("CapaN")> Gsol_Validaciones_IValidacion = 510
        <EnumMember> <Description("CapaN")> IBitacoras = 511
        <EnumMember> <Description("CapaN")> ICatalogo = 512
        <EnumMember> <Description("CapaN")> IConexiones = 513
        <EnumMember> <Description("CapaN")> IDocumento = 514
        <EnumMember> <Description("CapaN")> IExceptions64 = 515
        <EnumMember> <Description("CapaN")> IWSExtranetSuite64 = 516
        <EnumMember> <Description("CapaN")> IWSReferences64 = 517
        <EnumMember> <Description("CapaN")> IWSSesion = 518
        <EnumMember> <Description("CapaN")> Krombase243 = 519
        <EnumMember> <Description("CapaN")> Metas64 = 520
        <EnumMember> <Description("CapaN")> modSistemas_Catalogo_Acciones64 = 521
        <EnumMember> <Description("CapaN")> modSistemas_Catalogo_Usuarios64 = 522
        <EnumMember> <Description("CapaN")> ParseNotification64 = 523
        <EnumMember> <Description("CapaN")> SingletonSQLServer64 = 524
        <EnumMember> <Description("CapaN")> SolicitudAnticipo64 = 525
        <EnumMember> <Description("CapaN")> TagWatcher64 = 526
        <EnumMember> <Description("CapaN")> TagWatcherWCF64 = 527
        <EnumMember> <Description("CapaN")> Wcl000Catalogo = 528
        <EnumMember> <Description("CapaN")> WclComboBoxFinder64 = 529
        <EnumMember> <Description("CapaN")> WclTagBarControl64 = 530
        <EnumMember> <Description("CapaN")> WclTextBoxFinder64 = 531
        <EnumMember> <Description("CapaN")> WebServiceConsumerLib = 532
        <EnumMember> <Description("CapaN")> WebServiceConsumerLib_XmlSerializers = 533
        <EnumMember> <Description("CapaN")> Wma_Components_ComponentCriteria64 = 534
        <EnumMember> <Description("CapaN")> Wma_Components_DateTimePickerDCKR64 = 535
        <EnumMember> <Description("CapaN")> Wma_Components_IOperationsDynamicForm = 536
        <EnumMember> <Description("CapaN")> Wma_Components_ObjetoPregunta64 = 537
        <EnumMember> <Description("CapaN")> Wma_Components_OperationPairControls64 = 538
        <EnumMember> <Description("CapaN")> Wma_Components_OperationsDynamicForm64 = 539
        <EnumMember> <Description("CapaN")> Wma_Components_TextBoxDCKR64 = 540
        <EnumMember> <Description("CapaN")> Wma_Components_WclPoll64 = 541
        <EnumMember> <Description("CapaN")> Wma_Exceptions_TagWatcherEventMonitor64 = 542
        <EnumMember> <Description("CapaN")> Wma_Modulos_AtributosComplementosPago3364 = 543
        <EnumMember> <Description("CapaN")> Wma_Modulos_ComplementosPago3364 = 544
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesConceptosFactura3364 = 545
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesConceptosFactura64 = 546
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesFacturacion3364 = 547
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesFacturacion64 = 548
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesFacturacionCFDi3364 = 549
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesItemsPagosHechosFacturas64 = 550
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesRefFact3364 = 551
        <EnumMember> <Description("CapaN")> Wma_Modulos_GesRefFact64 = 552
        <EnumMember> <Description("CapaN")> Wma_ModulosComplementosPago3364 = 553
        <EnumMember> <Description("CapaN")> Wma_Mx_Invoices_CFDiGenerator = 554
        <EnumMember> <Description("CapaN")> Wma_Mx_Invoices_CFDiGenerator33 = 555
        <EnumMember> <Description("CapaN")> Wma_Mx_Invoices_CFDiGenerator64 = 556
        <EnumMember> <Description("CapaN")> Wma_Mx_Invoices_IAddenda = 557
        <EnumMember> <Description("CapaN")> Wma_Reports_DesktopReports64 = 558
        <EnumMember> <Description("CapaN")> Wma_Reports_ExcelApplicationReports64 = 559
        <EnumMember> <Description("CapaN")> Wma_Reports_IReport = 560
        <EnumMember> <Description("CapaN")> Wma_Services_WSReferenceDashboard64 = 561
        <EnumMember> <Description("CapaN")> WSUserprofile64 = 562
        <EnumMember> <Description("CapaN")> XMLPrepedimento = 563
        <EnumMember> <Description("CapaN")> FormularioMaestroDetalle = 564

    End Enum

    Enum ConceptosPagos
        ImpuestosSegunPedimento = 36
        RectificacionDePedimento = 116
    End Enum

    Enum Beneficiarios
        TesoreriaDeLaFederacionATV = 3153
        TesoreriaDeLaFederacionDAI = 1319
        TesoreriaDeLaFederacionRKU = 516
        TesoreriaDeLaFederacionSAP = 1267
        TesoreriaDeLaFederacionALC = 46
        TesoreriaDeLaFederacionCEG = 274
        TesoreriaDeLaFederacionTOL = 186
    End Enum

    Enum ClaveEjecutivos
        desarrollo_kromaduanal_com = 138
    End Enum

    Enum ClaveUsuarios
        desarrollo_kromaduanal_com = 1246
    End Enum

    Enum GrupoDestinatariosSolicitudPagoOperativa
        SolicitudPagoOperativaRKU = 31
        SolicitudPagoOperativaATV = 32
        SolicitudPagoOperativaDAI = 33
        SolicitudPagoOperativaCEG = 34
        SolicitudPagoOperativaTOL = 35
        SolicitudPagoOperativaSAP = 36
        SolicitudPagoOperativaALC = 37
        SolicitudPagoOperativaLAR = 38
    End Enum

    Enum GrupoDestinatariosSolicitudPagoAdministrativa
        SolicitudPagoAdministrativaRKU = 8
        SolicitudPagoAdministrativaATV = 9
        SolicitudPagoAdministrativaDAI = 10
        SolicitudPagoAdministrativaCEG = 11
        SolicitudPagoAdministrativaTOL = 12
        SolicitudPagoAdministrativaSAP = 13
        SolicitudPagoAdministrativaALC = 14
        SolicitudPagoAdministrativaLAR = 15
    End Enum

    Enum GrupoDestinatariosGerenciasTrafico
        GerenciaTraficoRKU = 43
        GerenciaTraficoDAI = 44
        GerenciaTraficoCEG = 45
        GerenciaTraficoTOL = 46
        GerenciaTraficoSAP = 47
        GerenciaTraficoALC = 48
        GerenciaTraficoNL = 49
    End Enum

    Enum GiroEmpresarial
        AgenciaAduanal = 10
        Logistica = 11
        Reexpedidora = 38
    End Enum

    Enum TipoCuenta
        EjecutivoCliente = 1
        EjecutivoClienteAuxiliar = 2
        CuentaClave = 3
        GerenteTrafico = 4
        CordinadorTrafico = 5
        JefeTrafico = 6
        CapturistaPedimento = 7
        GerenteGeneralTrafico = 8
    End Enum

    Enum TipoCorreoSolicitudPago
        TraficoPrincipal = 4
        TraficoCopia = 5
        AdministracionPrincipal = 6
        AdministracionCopia = 7
    End Enum

    Enum TiposDocumento
        'SolicitudPago = 178 ' Pruebas
        SolicitudPago = 193 'Produccion
        EIR = 148
    End Enum

    Enum TipoAplicacion
        KromBase = 4
        KromBaseWeb = 12
    End Enum

    Structure TipoContabilidad
        Public Shared ReadOnly Nacional As String = "N"
        Public Shared ReadOnly Extranjera As String = "E"
    End Structure



    Enum ClavePermisos
        'Nombre (t_Permiso) y clave (i_Cve_Permiso) de permiso que se encuentra en el catalogo de permisos "Cat000Permisos"

        'Claves de permisos gráficos


        'Claves de permisos abstractos
        ConsultarAcciones = 118
        AgregarRecolectorDocumentos = 1014
        ModificarRecolectorDocumentos = 1015
        ConsultarRecolectorDocumentos = 1016
        ConsultarCaracteristicasDocumentosRecolector = 1017
        AgregarCaracteristicasDocumentosRecolector = 1018
        ModificarCaracteristicasDocumentosRecolector = 1019
        EliminarCaracteristicasDocumentosRecolector = 1020
        ConsultarTiposDocumentosRecolector = 1021
        AgregarTiposDocumentosRecolector = 1022
        ModificarTiposDocumentosRecolector = 1023
        ConsultarBitacoraRecolectorDocumentos = 1024
        ConsultaErroresRecolector = 1025
        KBWBusquedaGeneralPO = 1113
        KBWAdministradorSeguimiento = 1161
        KBWAutorizaciónSolicitudPago = 1247
        KBWAutorizaciónSolicitudPagoOperativa = 1501
        KBWAutorizaciónSolicitudPagoAdministrativa = 1502
        KBWlistasolicitudesdepagoRKU = 1248
        KBWlistasolicitudesdepagoDAI = 1249
        KBWlistasolicitudesdepagoTOL = 1250
        KBWlistasolicitudesdepagoALC = 1251
        KBWlistasolicitudesdepagoSAP = 1252
        KBWlistasolicitudesdepagoCEG = 1253
        KBWlistasolicitudesdepagoLAR = 1254
        KBWlistasolicitudesdepagoATV = 1255
        P040_MonitorTargets_EjecutivoMultiOficina = 1330    'pro 1330   pru 2311
        P040_MonitorTargets_AdminOficina = 1332             'pro 1332   pru 2312
        P040_MonitorTargets_AdminMultiOficina = 1331        'pro 1331   pru 2313
        P003_KardexClientes_Admin = 1346                    'pro 1346   pru 2292
        PermisoEspecialCuestionariosClasificacion = 1369
        PermisoEspecialRevisionFraccionArancelaria = 1378
        P026_CausalOperativa = 1382                        'pro 1382   pru 2314
        P026_CausalAdministrativa = 1383                    'pro 1383   pru 2315
        ClientesOtrosDatosMultioficina = 1384
        P026_CausalOperativa_Incidencia = 1386              'pro 1386   pru 2317
    End Enum

    Enum Politicas

        ObtenerDuracionViajeImportacion = 33
        ObtenerClavePlantillaDocumento = 37
        VerificarOperacionRevalidada = 38
        'VerificarExistaDocumentoEnMD = 39 Pruebas
        VerificarExistaDocumentoEnMD = 40 'Producción
        ObtenerCalculoProfit = 43
        VerificarCPEmitidosFactura = 44 'Modificar para produccion XD
        ActualizarFechaEntrada = 45
        VerificaPedimentoValidado = 46
        VerificaTipoPedimento = 47
        InsertarContenedorEnPedimento = 48
        InsertarRemesaEnPedimento = 49
        ModificarContenedorEnPedimento = 50
        ModificarRemesaEnPedimento = 51
        EliminarContenedorEnPedimento = 52
        EliminarRemesaEnPedimento = 53
        FinanciamientoPorTipoMovimientoMonto = 54
        ConsultaPolizasCanceladas = 55

    End Enum

    Enum Monitor

        Trafico = 1
        Administrativo = 2

    End Enum

    Enum TipoDeFolio

        CuentaGastos = 1
        MaestroOperaciones = 2

    End Enum

    Enum EventosNotificacion

        Facturacion = 1
        RechazoVoboClasificacion = 2

    End Enum

    Enum TipoNotificacion

        Programada = 1
        BajoDemanda = 2

    End Enum

    Enum TipoPlantillasCorreo

        CorreoConEnlace = 1
        CorreoSencillo = 2

    End Enum

    Enum TipoOperacion

        Importacion = 1
        Exportacion = 2

    End Enum

    Enum TipoModalidad

        SinDefinir = 1
        Maritimo = 2
        Aereo = 3
        Terrestre = 4

    End Enum

    Enum Nave

        SinDefinir = 4722

    End Enum

    Enum TipoPedimento

        Normal = 1
        Consolidado = 2

    End Enum

    Enum EstatusRevalidacion

        Pendiente = 1
        RevalidadoEnKB = 2
        RevalidadoEnSysExpert = 3

    End Enum

    Enum EstatusViaje

        Abierto = 1
        Cerrado = 2

    End Enum

    Enum MovimientosCarteraOrigen

        MovimientosCartera10 = 1
        MovimientosCartera20 = 2

    End Enum

    Enum FormatoCuentaGastos

        PorDefecto = 0
        AgrupamientoPagosHechosPorRazonSocial = 1
        FormatoIngles = 2
        SinAgrupamientoConceptos = 3
        DetalleCompletoPT = 4
        ConceptoPagosHechos = 5

    End Enum

#End Region

#Region "Metodos"

    'Function ObtenerPermisos(ByVal tipoAplicacion_ As TipoAplicacion,
    '                         ByVal tipoModulo_ As IEspacioTrabajo.TipoModulo,
    '                         Optional clavePermiso_ As ClavePermisos = ClavePermisos.ConsultarAcciones_118) As TagWatcher

#End Region

End Interface