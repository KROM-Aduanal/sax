<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MaintainScrollPositionOnPostback="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-MetaforaPedimento.aspx.vb" Inherits=".Ges022_001_MetaforaPedimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <style>
        .CP_MODALIDAD_ADUANA_PATENTE .wc-select {
            top: 50%;
            -webkit-transform: translateY(-50%); /* WebKit */
            -moz-transform: translateY(-50%); /* Mozilla */
            -o-transform: translateY(-50%); /* Opera */
            -ms-transform: translateY(-50%); /* Internet Explorer */
            transform: translateY(-50%); /* CSS3 */
            z-index: 1;
        }

        .cl_totales__Total, .partidas__secuencia {
            background-color: #673ab7;
            opacity: .6;
            color: #fff;
            display: inline-block;
            padding: 0 14px;
            border-radius: 0 16px 16px 0px;
        }

        .cl_totales__Total {
            font-size: 2.4em;
            font-weight: bold;
        }

        .partidas__secuencia {
            font-size: 1.8em;
        }

        .cl_totales__moneda {
            font-weight: 100;
        }

        .cl_totales__montos input {
            font-size: 26px;
            text-align: right;
            color: #757575;
        }

        .cl_totales__montos label {
            left: initial;
            right: 0;
        }

        .linea_captura > legend {
            font-size: 18px;
        }

        .linea_captura__rqcode .wc-image {
            display: flex;
            justify-content: center;
        }

        .partidas__fraccion input {
            font-size: 24px;
            color: #757575;
        }

        .partidas__fraccion-contenedor {
            padding: 20px 8px;
        }

        .partidas__descripción {
            color: #757575;
        }
    </style>

    <% If IsPopup = False Then %>

    <GWC:FindbarControl runat="server" ID="__SYSTEM_CONTEXT_FINDER" Label="Buscar" OnClick="BusquedaGeneral"/>

    <% End If %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
    <% If IsPopup = False Then %>

    <GWC:SelectControl runat="server" ID="__SYSTEM_ENVIRONMENT" CssClass="col-auto company-list-select" SearchBarEnabled="false" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

    <div class="d-flex">

        <GWC:FormControl HasAutoSave="true" runat="server" Label="Modulo de pedimento" ID="__SYSTEM_MODULE_FORM" OnCheckedChanged="MarcarPagina">

            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Manifestación de Valor" />                    
                    <GWC:ButtonItem Text="Descarga MV" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>

                <GWC:FieldsetControl runat="server" ID="fsc_DatosGenerales" Label="Generales">

                    <ListControls>

                        <%--CP_REFERENCIA--%>
                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="dbc_ReferenciaPedimento" Label="Referencia aduanal" LabelDetail="Número de pedimento" />

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 d-flex align-items-center mb-5">

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 pl-0" ID="sc_TipoReferencia" Label="Tipo ref." OnSelectedIndexChanged="sc_TipoReferencia_SelectedIndexChanged">
                                <Options>
                                    <GWC:SelectOption Value="1" Text="Operativa" />
                                    <GWC:SelectOption Value="2" Text="Corresponsalía" />
                                    <GWC:SelectOption Value="3" Text="Corresponsalía terceros" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 pr-0" ID="sc_PrefijoReferencia" Label="Prefijo" OnSelectedIndexChanged="sc_PrefijoReferencia_SelectedIndexChanged" />

                        </asp:Panel>

                        <GWC:SelectControl runat="server" CssClass="CP_MODALIDAD_ADUANA_PATENTE col-xs-12 col-md-6 mb-5" ID="sc_Patente" Label="Patente" OnClick="sc_Patente_Click">                            
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" ID="sc_EjecutivoCuenta" Label="Ejecutivo de cuenta" KeyField ="i_Cve_EjecutivosMisEmpresas" DisplayField ="t_NombreCompleto" Dimension ="EjecutivosMiEmpresa">                           
                        </GWC:SelectControl>

                        <%--CA_CVE_TIPO_OPERACION--%>
                        <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 d-flex align-items-center jc-center" ID="sch_TipoOperacion" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_ClavePedimento" Label="Clave Pedimento" KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-6" ID="sc_Regimen" Label="Régimen" KeyField="t_Cve_Regimen" DisplayField="t_DescripcionCorta" Dimension="Vt022RegimenesA16" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_DestinoMercancia" Label="Destino de la mercancia" KeyField="i_ClaveDestinoMercancia" DisplayField="t_DescripcionDestinoMercancia" Dimension="Vt022DestinosMercanciasA15" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_TipoCambio" Format="Money" Label="Tipo de cambio" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_PesoBruto" Label="Peso bruto" Rules="onlynumber" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_AduanaEntradaSalida" Label="Aduana de entrada/salida" Enabled="false" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_TransporteEntradaSalida" Label="Transporte entrada/salida" KeyField="t_Cve_MedioTransporte" DisplayField="t_MedioTransporte" Dimension="Vt022MediosTransporteA03" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_MedioTransporteArribo" Label="Medio de transporte/arribo" Enabled="false" KeyField="t_Cve_MedioTransporte" DisplayField="t_MedioTransporte" Dimension="Vt022MediosTransporteA03" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_MedioTransporteSalida" Label="Medio de transporte /salida" KeyField="t_Cve_MedioTransporte" DisplayField="t_MedioTransporte" Dimension="Vt022MediosTransporteA03" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="ic_ValorDolares" Label="Valor en dólares" Format="Money" Enabled="false" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="ic_ValorAduana" Label="Valor aduana" Enabled="false" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_PrecioPagado" Label="Precio pagado/Valor comercial" Enabled="false" />

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="DatosImportador" Detail="Datos del importador/exportador" Label="Datos Imp/Exp">

                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-md-12 p-0">

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:FindboxControl runat="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="fbc_Cliente" Label ="Cliente" KeyField ="_id" DisplayField ="CA_RAZON_SOCIAL" OnTextChanged="fbc_Cliente_TextChanged" OnClick="fbc_Cliente_Click" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="ic_RFCCliente" Label="RFC" Enabled="false" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="ic_CURP" Label="CURP" Enabled="false" />

                            </div>

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="ic_RFCFacturacion" Label="RFC facturación" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="TextArea" ID="ic_DomicilioCliente" Label="Domicilio" Enabled="false" />

                            </div>

                        </asp:Panel>

                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_CALLE_IOE" Label="Calle" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="CA_NUM_INTER_IOE" Label="# interior" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="CA_NUM_EXTERIOR_IOE" Label="# exterior" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="CA_CODIGO_POSTAL_IOE" Label="C.P." />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="CA_MUNICIPIO_CIUDAD_IOE" Label="Municipio">
                            <Options>
                                <GWC:SelectOption Text="Selecciona" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="CA_ENTIDAD_FEDERATIVA_IOE" Label="Ent. federativa">
                            <Options>
                                <GWC:SelectOption Text="Selecciona" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="CA_PAIS_IOE" Label="País">
                            <Options>
                                <GWC:SelectOption Text="Selecciona" Value="1" />
                            </Options>
                        </GWC:SelectControl>--%>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="ic_ValorSeguros" Label="Valor seguros" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="ic_Seguros" Label="Seguros dec." />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="ic_Fletes" Label="Fletes" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="ic_Embalajes" Label="Embalajes" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="ic_OtrosIncrementables" Label="Otros incrementables" />

                        <asp:Panel runat="server" CssClass="decrementable-box col-xs-12 mb-5">

                            <asp:Panel runat="server" CssClass="row fieldset " fieldset-legend="Valor decrementables">

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="ic_TransporteDec" Label="Transporte dec." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="ic_SegurosDec" Label="Seguro dec." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="ic_CargaDec" Label="Carga" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3" Type="Text" ID="ic_DescargaDec" Label="Descarga" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3" Type="Text" ID="ic_OtrosDec" Label="Otros decrementables" />

                            </asp:Panel>

                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="ValidacionPago" Detail="Validación y pago" Label="Validación">

                    <ListControls>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_ValidadorDesignado" Label="Validador asignado">
                            <Options>
                                <GWC:SelectOption Text="Seleccionar" Value="" />
                                <GWC:SelectOption Text="algo" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_NumeroSemana" Label="Número de semana">
                            <Options>
                                <GWC:SelectOption Text="Seleccionar" Value="0" />
                                <GWC:SelectOption Text="algo" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_ArchivoValidacion" Label="Archivo de validación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_AcuseValidación" Label="Respuest de validación/Acuse electrónico de validación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_ArchivoPago" Label="Archivo de pago" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_AcusetaPago" Label="Respuesta de pago/Acuse electrónico de pago" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_ValidacionAduanaDespacho" Label="Aduana de despecho">
                            <Options>
                                <GWC:SelectOption Text="Seleccionar" Value="0" />
                                <GWC:SelectOption Text="430 - Vercruz, Ver, México" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 nopadding">

                            <label class="info-label pl-5">Código de barra</label>

                            <GWC:ImageControl CssClass="col-xs-12" runat="server" Source="/FrontEnd/Recursos/Imgs/barcode.jpg" Height="60px" Width="350px" Aspect="Cover" />

                        </asp:Panel>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5 d-flex justify-content-center align-items-center" Type="Text" ID="ic_MarcasNumeros" Label="Marcas, Números y total de bultos" />

                        <%--ESTOS CAMPOS NO APARECEN EN LA METAFORA DEL mockflow--%>
                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_CLAVE_SAD" Label="Sección aduanera de despacho" />--%>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_Certificacion" Label="Certificación" />

                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_AÑO_VALIDACION_2" Label="Año de validación 2" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_ADUANA_SIN_SECCION" Label="Aduana sin sección" />--%>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="ic_FechaValidacion" Label="Fecha de Validación" />

                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CP_MODALIDAD" Label="Modalidad" />--%>
                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Fechas" Detail="Fechas" Label="Fechas">

                    <ListControls>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaRegistro" Label="Registro" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaRevalidacion" Label="Revalidación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaZarpe" Label="Zarpe" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaPrevio" Label="Previo" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaFondeo" Label="Fondeo" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaPago" Label="pago de pedimento" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaAtraque" Label="Atraque" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaDespacho" Label="Despacho" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaEstimadaArribo" Label="Estimada de arribo" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaEntrega" Label="Entrega" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaEntrada" Label="Entrada" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="ic_FechaFacturacion" Label="Facturación" />

                        <%--ESTOS CAMPOS NO APARECEN EN LA METAFORA DEL mockflow--%>
                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_EXTRACCION" Label="Extracción" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_PRESENTACION" Label="Presentación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_IMP_EUA_CAN" Label="IMP EUA CAN" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_ORIGINAL" Label="Original" />--%>
                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="TasasContribuciones" Detail="Tasas y totales" Label="Tasas y totales">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoTasas" runat="server" KeyField="Catalogo_tasas">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_TasasContribucion" Label="Contribución" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_TasasTipoTasa" Label="Cve. tipo tasa" KeyField="i_ClaveTipoTasa" DisplayField="t_DescripcionTipoTasa" Dimension="Vt022TiposTasasA18" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_TasasTasa" Label="Tasa" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="CuadroLiquidacion" Detail="Cuadro de liquidación" Label="Cuadro de liquidación">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoCuadroLiquidacion" runat="server" KeyField="Catalogo_liquidacion" CssClass="bold">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CuadroLiquidacionConcepto" Label="Concepto" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CuadroLiquidacionDescripcion" Enabled="false" Label="Descripción" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CuadroLiquidacionFP" Label="Forma de pago" KeyField="i_ClaveFormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CuadroLiquidacionImporte" Label="Importe" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                        <asp:Panel runat="server" CssClass="col-md-12 mb-5 nopadding cl_totales">

                            <div class="col-xs-12 col-md-3">
                                <div class="cl_totales__Total">
                                    Total <span class="cl_totales__moneda">MXN</span>
                                </div>
                            </div>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 cl_totales__montos" Type="Text" ID="ic_CuadroLiquidacionEfectivo" Format="Money" Label="Efectivo" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 cl_totales__montos" Type="Text" ID="ic_CuadroLiquidacionOtros" Format="Money" Label="Otros" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 cl_totales__montos" Type="Text" ID="ic_CuadroLiquidacionTotal" Format="Money" Label="Total" />

                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Cetificacion" Detail="Certificación" Label="Certificación">

                    <ListControls>

                        <GWC:FieldsetControl runat="server" CssClass="linea_captura" ID="LineaCaptura" Detail="Depósito referenciado - línea de captura - información del pago" Label="Línea de captura">

                            <ListControls>

                                <asp:Panel runat="server" CssClass="col-md-12">

                                    <div class="col-xs-12 col-md-6">

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 nopadding">

                                            <label class="info-label pl-5">Código de barra</label>

                                            <GWC:ImageControl CssClass="col-xs-12" runat="server" Source="/FrontEnd/Recursos/Imgs/barcode.jpg" Height="60px" Width="350px" Aspect="Cover" />

                                        </asp:Panel>

                                        <h3 class="w-100 d-flex py-3 justify-content-center">**Pago electrónico**</h3>

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4" Type="Text" ID="ic_LineaCapturaPatente" Label="Patente" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4" Type="Text" ID="ic_LineaCapturaPedimento" Label="Pedimento" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4" Type="Text" ID="ic_LineaCapturaAduana" Label="Aduana" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaBanco" Label="Banco" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaNumero" Label="Línea de captura" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaImporte" Format="Money" Label="Importe pagado" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaPago" Format="Calendar" Label="Fecha de pago" />

                                    </div>

                                    <div class="col-xs-12 col-md-6">

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaNumeroOperacion" Label="Número de operación bancaria" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaNumeroTransaccion" Label="Número de transacción al SAT" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaPresentacion" Label="Medio de presentación" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_LineaCapturaRecepsion" Label="Medio de recepsión/cobro" />

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 linea_captura__rqcode">

                                            <GWC:ImageControl CssClass="col-xs-12" runat="server" Source="/FrontEnd/Recursos/Imgs/rqcode.png" Height="200px" Width="200px" Aspect="Cover" />

                                        </asp:Panel>

                                    </div>

                                </asp:Panel>

                            </ListControls>

                        </GWC:FieldsetControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="DatosProveedoresImpo" Detail="Proveedores" Label="Datos de proveedores">

                    <ListControls>

                        <%--<GWC:PaginatorControl ID="pgDatosProveedores" runat="server" NumberItems="20" ItemsPage="5" CssClass="col-auto ml-auto mb-5" />--%>

                        <%--<asp:Panel runat="server" CssClass="row col-md-12">

                            <GWC:PillboxControl runat="server" ID="pb_Proveedores" KeyField="i_Cve_PbProveedores" CssClass="col-md-12">

                                <ListControls>

                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 p-0">

                                        <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="Text" ID="ic_IdFiscalProveedor" Label="ID Fiscal" Enabled="false" />

                                        <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="ic_NombreProveedor" Label="Nombre, denominación o razón social">
                                            <Options>
                                                <GWC:SelectOption Text="Samsung electronics S.A. DE C.V." Value="1" />
                                            </Options>
                                        </GWC:SelectControl>

                                    </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 p-0">

                                        <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="TextArea" ID="ic_DocimilioProveedor" Label="Domicilio" Enabled="false" />

                                    </asp:Panel>

                                </ListControls>

                            </GWC:PillboxControl>

                            <asp:Panel runat="server">--%>

                                <GWC:CatalogControl ID="CatalogoFacturas" runat="server" KeyField="Catalogo_facturas" CssClass="col-md-12 mb-5 p-0">

                                    <Columns>

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_FacturaProveedor" Label="Factura" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_FechaFacturaProveedor" Label="Fecha" Format="Calendar" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_IncontermProveedor" Label="Inconterm" />

                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_FactorMonedaProveedor" Label="Factor moneda">
                                            <Options>
                                                <GWC:SelectOption Text="Selecciona" Value="1" />
                                            </Options>
                                        </GWC:SelectControl>

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_MontoFacturaProveedor" Label="Monto" Format="Money" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_MontoFacturaUSDProveedor" Label="Monto USD" Format="Money" />

                                    </Columns>

                                </GWC:CatalogControl>

                            <%--</asp:Panel>--%>

                            <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="CA_CALLE_POC" Label="Calles" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="CA_NUMERO_INT_POC" Format="Numeric" Label="# Interior" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="CA_NUMERO_EXTER_POC" Format="Numeric" Label="# Exterior" />

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="CA_CODIGO_POSTAL_POC" Label="C.P.">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="CA_MUNICIPIO_CIUDAD_POC" Label="Municipio">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="CA_ENTIDAD_FEDERATIVA_POC" Label="Entidad federativa">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>--%>
                        <%--</asp:Panel>--%>

                        <%--<GWC:CatalogControl ID="CatalogoProveedores" runat="server" KeyField="Catalogo_proveedor" CssClass="col-md-12 mt-5 p-0">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_COVE" Label="COVE">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-12 d-flex align-items-center jc-center" ID="CA_VINCULACION" Label="Vinculación" OnText="Si" OffText="No" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="proveedor_CA_INCOTERM" Label="INCOTERM">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                            </Columns>

                        </GWC:CatalogControl>--%>
                    </ListControls>

                </GWC:FieldsetControl>

                <%--<GWC:FieldsetControl runat="server" ID="DatosProveedoresExpo" Detail="Datos proveedores / exportación" Label="Datos de proveedores expo">

                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-md-12 p-0">

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="Text" ID="proveedorExpo_CA_ID_FISCAL_PROVEEDOR" Label="ID Fiscal" Enabled="false" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="proveedorExpo_CA_NOMBRE_DEN_RAZON_SOC_POC" Label="Nombre, denominación o razón social">
                                    <Options>
                                        <GWC:SelectOption Text="Samsung electronics S.A. DE C.V." Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                            </div>

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="TextArea" ID="proveedorExpo_CA_DOMICILIO_POC" Label="Domicilio" Enabled="false" />

                            </div>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="proveedorExpo_CA_CALLE_POC" Label="Calles" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="proveedorExpo_CA_NUMERO_INT_POC" Format="Numeric" Label="# Interior" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="proveedorExpo_CA_NUMERO_EXTER_POC" Format="Numeric" Label="# Exterior" />

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="proveedorExpo_CA_CODIGO_POSTAL_POC" Label="C.P.">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="proveedorExpo_CA_MUNICIPIO_CIUDAD_POC" Label="Municipio">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="proveedorExpo_CA_ENTIDAD_FEDERATIVA_POC" Label="Entidad federativa">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>
                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>--%>

                <%--<GWC:FieldsetControl runat="server" ID="DatosProveedores" Detail="Datos proveedores" Label="Datos de proveedores">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoProveedores" runat="server" KeyField="Catalogo_proveedor">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_ID_FISCAL_PROVEEDOR" Label="Taxt ID" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="CA_NOMBRE_DEN_RAZON_SOC_POC" Label="Proveedor">
                                    <Options>
                                        <GWC:SelectOption Text="Samsung electronics S.A. DE C.V." Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_DOMICILIO_POC" Label="Domicilio" />

                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-12 d-flex align-items-center jc-center" ID="CA_VINCULACION" Label="Vinculación" OnText="Si" OffText="No" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_CALLE_POC" Label="Calles" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUMERO_INT_POC" Format="Numeric" Label="# Interior" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUMERO_EXTER_POC" Format="Numeric" Label="# Exterior" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_CODIGO_POSTAL_POC" Label="C.P.">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_MUNICIPIO_CIUDAD_POC" Label="Municipio">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_ENTIDAD_FEDERATIVA_POC" Label="Entidad federativa">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>--%>

                <GWC:FieldsetControl runat="server" ID="Destinatarios" Detail="Datos destinatarios / exportación" Label="Destinatarios">

                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-md-12 p-0">

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="Text" ID="ic_TaxtIDDestinatario" Label="Taxt ID" Enabled="false" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="sc_RazonSocialDestinatario" Label="Destinatario">
                                    <Options>
                                        <GWC:SelectOption Text="Samsung electronics S.A. DE C.V." Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                            </div>

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="TextArea" ID="ic_DomicilioDestinatario" Label="Domicilio" Enabled="false" />

                            </div>

                            <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="CA_CALLE_DESTINATARIO" Label="Calles" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="CA_NUMERO_INT_DESTINATARIO" Label="# Interior" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="CA_NUMERO_EXTER_DESTINATARIO" Label="# Exterior" />

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="CA_CODIGO_POSTAL_DESTINATARIO" Label="C.P.">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="CA_MUNICIPIO_CIUDAD_DESTINATARIO" Label="Municipio">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="CA_PAIS_DESTINATARIO" Label="Entidad federativa">
                                <Options>
                                    <GWC:SelectOption Text="Selecciona" Value="1" />
                                </Options>
                            </GWC:SelectControl>--%>
                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>

                <%--<GWC:FieldsetControl runat="server" ID="Destinatarios" Detail="Destinatarios" Label="Destinatarios">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoDestinatarios" runat="server" KeyField="Catalogo_destinatarios">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_ID_FISCAL_DESTINATARIO" Label="Taxt ID" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="CA_NOMBRE_RAZON_SOC_DESTINATARIO" Label="Destinatario">
                                    <Options>
                                        <GWC:SelectOption Text="Samsung electronics S.A. DE C.V." Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_DOMICILIO_DESTINATARIO" Label="Domicilio" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_CALLE_DESTINATARIO" Label="Calles" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUMERO_INT_DESTINATARIO" Label="# Interior" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUMERO_EXTER_DESTINATARIO" Label="# Exterior" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_CODIGO_POSTAL_DESTINATARIO" Label="C.P.">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_MUNICIPIO_CIUDAD_DESTINATARIO" Label="Municipio">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_PAIS_DESTINATARIO" Label="Entidad federativa">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>--%>

                <GWC:FieldsetControl runat="server" ID="DatosTransporte" Detail="Datos del transporte" Label="Transporte">

                    <ListControls>

                        <%--<GWC:ToolbarControl runat="server" CssClass="col-auto ml-auto mb-5" ID="tbc_DatosTransporte" ShowTitles="true" OnClick="tbc_DatosTransporte_Click" OnCheckedChanged="tbc_DatosTransporte_CheckedChanged" />--%>

                        <%--<GWC:PillboxControl runat="server" ID="pb_DatosTransporte" CssClass="col-md-12" KeyField="i_Cve_PbTransporte" >

                            <ListControls>

                                <asp:Panel runat="server" CssClass="col-md-12 p-0 py-5 mt-0">

                                    <div class="col-xs-12 col-md-6 mt-5 p-0">

                                        <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="Text" ID="ic_IDTransporte" Label="Identificación" />

                                        <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="sc_PaisTransporte" Label="País" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                    </div>

                                </asp:Panel>

                            </ListControls>

                        </GWC:PillboxControl>--%>

                        <asp:Panel runat="server">

                            <div class="col-xs-12 col-md-6 mt-5 p-0">

                                <GWC:CatalogControl ID="CatalogoCandados" runat="server" KeyField="Catalogo_candados" CssClass="col-md-12">

                                    <Columns>

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_NumeroCandado" Label="Candado" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_CandadoPrimeraRevisión" Label="Primera revisión" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_CandadoSegundaRevision" Label="Segunda revisión" />

                                    </Columns>

                                </GWC:CatalogControl>

                            </div>

                        </asp:Panel>

                        <%--ESTOS CAMPOS NO APARECEN EN LA METAFORA DEL mockflow---%>
                        <%--<GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="CA_NOMBRE_RAZON_SOC_TRANSP" Label="Transportista">
                            <Options>
                                <GWC:SelectOption Text="Auto Express Hércules, S.A. de C.V." Value="1" />
                                <GWC:SelectOption Text="Fletes Sotelo, S.A. de C.V." Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" ID="CA_CVE_PAIS_TRANSP" Label="País">
                            <Options>
                                <GWC:SelectOption Text="01 - México" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="CA_CVE_RFC_TRANSP" Label="RFC" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="CA_CURP_TRANSP_PERSONA_FISICA" Label="CURP" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="CA_TOTAL_BULTOS" Label="Total bultos" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-8 mb-5" Type="Text" ID="CA_DOMICILIO_TRANSP" Label="Domicilio" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-8 mb-5" Type="Text" ID="CA_ID_TRANSPORT" Label="Identificación" />--%>
                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Guias" Detail="Guias, manifiestos / conocimientos de embarque" Label="Guías">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoGuias" runat="server" KeyField="Catalogo_guias" CssClass="row col-auto">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="ic_Guia" Label="Guía, manifiestos o BL" />

                                <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_MASTER_O_HOUSE" Label="Guía maester o house" />--%>

                                <%--<GWC:SelectControl runat="server" CssClass="col-md-12 mt-5" ID="" Label="Número de guía | conocimiento embarque">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>--%>

                                <GWC:SwitchControl runat="server" CssClass="col-md-12 mt-5 d-flex align-items-center jc-center" ID="sc_TipoGuia" Label="ID (Tipo de guía) " OnText="Master" OffText="House" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Contenedores" Detail="Contenedores" Label="Contenedores">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoContenedores" runat="server" KeyField="Catalogo_contenedores">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_NumeroContenedor" Label="Número  de cont | Equipo ferrocarril | Núm. económico" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_TipoContenedor" Label="Tipo" KeyField="i_ClaveTipoContenedorVehiculoTransporte" DisplayField="t_DescripcionTipoContenedorVehiculoTransporte" Dimension="Vt022TiposContenedoresVehiculosTransporteA10" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <%--<GWC:FieldsetControl runat="server" ID="Candados" Detail="Candados" Label="Candados">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogCandados" runat="server" KeyField="Catalogo_candados">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUM_CANDADO" Label="Número candado" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUM_CANDADO_1RA" Label="Primera revisión" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUM_CANDADO_2DA" Label="Segunda revisión" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>--%>

                <%--<GWC:FieldsetControl runat="server" ID="Facturas" Detail="Facturas" Label="Facturas">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoFacturas" runat="server" KeyField="Catalogo_facturas">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_CFDI_O_FACT" Label="Factura" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" Format="Calendar" ID="CA_FECHA_FACT" Label="Fecha" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_INCOTERM" Label="Inconterm" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="CA_FACTOR_MONEDA" Label="Factor moneda">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" Format="Money" ID="CA_MONTO_MONEDA_FACT" Label="Monto" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_MONTO_USD" Format="Money" Label="Monto USD" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="InpuCA_NUMERO_ACUSE_DE_VALORtControl7" Label="# Exterior" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>--%>

                <GWC:FieldsetControl runat="server" ID="Identificadores" Detail="Identificadores" Label="Identificadores">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoIdentificadores" runat="server" KeyField="Catalogo_identificadores">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_IdentificadorPedimento" Label="Clave" KeyField="t_Cve_Identificador" DisplayField="t_Identificador" Dimension="Vt022IdentificadoresA08" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_Complemento1Pedimento" Label="Complemento 1" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_Complemento2Pedimento" Label="Complemento 2" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_Complemento3Pedimento" Label="Complemento 3" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="CuentasAduaneras" Detail="Cuentas aduaneras" Label="Cuentas aduaneras">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoCuentasAduaneras" runat="server" KeyField="Catalogo_cuentasAduaneras">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CuentaAduanera" Label="Cuenta aduanera">
                                    <Options>
                                        <GWC:SelectOption Text="Cuenta aduanera de garantía global" Value="2" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_TipoCuentaAduanera" Label="Tipo garantia">
                                    <Options>
                                        <GWC:SelectOption Text="Depósito" Value="1" />
                                        <GWC:SelectOption Text="Fideicomiso" Value="2" />
                                        <GWC:SelectOption Text="Línea de Crédito" Value="3" />
                                        <GWC:SelectOption Text="Cuenta referenciada (depósito referenciado)" Value="4" />
                                        <GWC:SelectOption Text="Prenda" Value="5" />
                                        <GWC:SelectOption Text="Hipoteca" Value="6" />
                                        <GWC:SelectOption Text="Títulos valor" Value="7" />
                                        <GWC:SelectOption Text="Carteras de créditos del propio contribuyente" Value="8" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_InstitucionEmisora" Label="Emisora">
                                    <Options>
                                        <GWC:SelectOption Text="BBVA México, S.A., Institución de Banca Múltiple, Grupo Financiero BBVA México." Value="1" />
                                        <GWC:SelectOption Text="Banco Nacional de México, S.A." Value="2" />
                                        <GWC:SelectOption Text="Banco HSBC, S.A. de C.V." Value="3" />
                                        <GWC:SelectOption Text="Bursamex, S.A. de C.V." Value="4" />
                                        <GWC:SelectOption Text="Operadora de Bolsa, S.A. de C.V." Value="5" />
                                        <GWC:SelectOption Text="Vector Casa de Bolsa, S.A. de C.V." Value="6" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_NumeroCOntratoCuentaAduanera" Label="# contrato 3" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_FolioConstanciaCuentaAduanera" Label="Folio" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_ImporteCuentaAduanera" Label="Importe" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_FechaEmisionCuentaAduanera" Label="Emisión" Format="Calendar" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_PrecioEstimadoCuentaAduanera" Label="Precio estimado" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_TitulosCuentaAduanera" Label="Titulos" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_ValorUnitario" Label="Valor unitario" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Pagosvirtuales" Detail="Formas de pago virtual" Label="Pago virtual">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoPagosvirtuales" runat="server" KeyField="Catalogo_pagosvirtuales">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_PagosVirtualesFormaPago" Label="Forma de pago" KeyField="i_ClaveFormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_PagosVIrtualesEmisora" Label="Emisora">
                                    <Options>
                                        <GWC:SelectOption Text="Seleccionar" Value="0" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_PagosVirtualesDocumento" Label="# documento" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_PagosVirtualesFechaDocumento" Label="Fecha doc." Format="Calendar" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_PagosVirtualesImporteDocumento" Label="Importe" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_PagosVirtualesSaldo" Label="Saldo" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_PagosVirtualesImportePedimento" Label="Importe" Format="Numeric" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Rectificaciones" Detail="Rectificaciones" Label="Rectificaciones" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoRectificaciones" runat="server" KeyField="Catalogo_rectificaciones" UserInteraction="false">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_RectificacionesFechaPedOriginal" Label="Fecha ped. ori." Format="Calendar" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_RectificacionesClavePedOriginal" Label="Clave Ped. ori." KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_RectificacionesPatentePedOriginal" Label="Patente ori." />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_RectificacionesPedimentoCompleto" Label="Pedimento ori." />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_RectificacionesAñoValidacion2PedOriginal" Label="Año val. ori. 2" />
                                <%--Format="Numeric"--%>

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_RectificacionesAñoValidacionPedOriginal" Label="Año val. ori." />
                                <%--Format="Numeric"--%>

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_RectificacionesAduanaOriginal" Label="Aduana des. ori." KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_RectificacionesAduana2Original" Label="Aduana des. ori. 2" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_RectificacionesClavePedimento" Label="Clave Ped. recti." KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_RectificacionesFechaPedimento" Label="Fecha ped. recti." />
                                <%--Format="Calendar"--%>

                                <%--<GWC:SelectControl runat="server" CssClass="col-md-12" ID="Rectificaciones_CA_PATENTE_ORIGINAL" Label="Patente ori.">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="0" />
                                    </Options>
                                </GWC:SelectControl>--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="DiferenciaContribuciones" Detail="Diferencias de contribuciones" Label="Dif. contribuciones" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoDiferenciasRecti" runat="server" KeyField="Catalogo_DiferenciasRecti" UserInteraction="false">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_DifConConcepto" Label="Concepto" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_DifConFormaPago" Label="Forma de pago" KeyField="i_ClaveFormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DifConDiferencia" Label="Diferencia" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DifConEfectivo" Label="Efectivo" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DifConOtros" Label="Otros" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DifConTotal" Label="Dif totales" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Descargos" Detail="Descargos" Label="Descargos" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoDescargos" runat="server" KeyField="Catalogo_descargos">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosPedCompletoOriginal" Label="Pedimento" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosFechaPedOriginal" Label="Original" Format="Calendar" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_DescargosClavePedOriginal" Label="Clave" KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosValidacionOriginal" Label="Año val. original" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosValidacion2Original" Label="Año val. original 2" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_DescargosPatenteOriginal" Label="Patente">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_DescargosAduanaOriginal" Label="Aduana des. ori." KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_DescargosAduana2Original" Label="Aduana des. ori. 2" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosPedOriginal" Label="Pedimento ori. 2" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosFraccionOriginal" Label="Fraccion original" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosUMOriginal" Label="UM original" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_DescargosUMDescargo" Label="UM descargo" Format="Numeric" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Compensaciones" Detail="Compensaciones" Label="Compensaciones" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoCompensaciones" runat="server" KeyField="Catalogo_compensaciones">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CompensacionesContribucion" Label="Contribución" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CompensacionesPedCompletoOriginal" Label="Pedimento ori. com." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CompensacionesFechaPagoPedOriginal" Label="Pago original" Format="Calendar" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CompensacionesGravamen" Label="Gravamen" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CompensacionesConcepto" Label="Concepto">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="0" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CompensacionesAñoValidacionOriginal" Label="Año val. ori." Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CompensacionesAñoValidacion2Original" Label="Año val. ori. 2" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CompensacionesPatenteOriginal" Label="Patente original">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="0" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CompensacionesAduanaOriginal" Label="Aduana des. ori." KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_CompensacionesAduana2Original" Label="Aduana des. ori. 2" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_CompensacionesPedOriginal" Label="Pedimento ori. 2" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="PruebaSuficiente" Detail="Prueba suficiente" Label="Prueba suficiente" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="CatalogoPruebaSuficiente" runat="server" KeyField="Catalogo_pruebaSuficiente">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_PruebaSuficientePaisDestino" Label="País destino" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="ic_PruebaSuficientePedEUACAN" Label="# ped. EUA/CAN." />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_PruebaSuficiente" Label="Pru. suficiente">
                                    <Options>
                                        <GWC:SelectOption Text="Copia del recibo, que compruebe el pago del impuesto de importación a los Estados Unidos de América o Canadá." Value="1" />
                                        <GWC:SelectOption Text="Copia del documento de importación en que conste que éste fue recibido por la autoridad aduanera de los Estados Unidos de América o Canadá." Value="2" />
                                        <GWC:SelectOption Text="Copia de una resolución definitiva de la autoridad aduanera de los Estados Unidos de América o Canadá, respecto del impuesto de importación correspondiente a la importación de que se trate." Value="3" />
                                        <GWC:SelectOption Text="Un escrito firmado por el importador en los Estados Unidos de América o Canadá o por su representante legal." Value="4" />
                                        <GWC:SelectOption Text="Un escrito firmado bajo protesta de decir verdad, por la persona que efectúe el retorno o exportación de las mercancías o su representante legal con base en la información proporcionada por el importador en los Estados Unidos de América o Canadá o por su representante legal." Value="5" />
                                    </Options>
                                </GWC:SelectControl>

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Observaciones" Detail="Observaciones / a nivel pedimento" Label="Observaciones ped.">

                    <ListControls>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea" Type="TextArea" ID="ic_OnservacionesPedimento" Label="Observaciones" />

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Partidas" Detail="Partidas" Label="Partidas">

                    <ListControls>

                        <asp:Panel runat="server" CssClass="row col-md-12">

                            <%--<div class="col-xs-12 col-md-3">
                                <div class="partidas__secuencia">
                                    Secuencia
                                </div>
                            </div>--%>

                            <%--<GWC:PaginatorControl runat="server" ID="pgPatidas" NumberItems="50" ItemsPage="10" CssClass="col-xs-12 col-md-9 mb-5" />--%>
                            <asp:Panel runat="server" CssClass="col-md-12 d-flex mt-0">

                                <GWC:ToolbarControl runat="server" ID="ToolbarControl1" CssClass="col-auto ml-auto mb-5" Modality="Simple" ShowTitles="true" />

                            </asp:Panel>

                            <div class="col-xs-12 col-md-6 mb-5">

                                <asp:Panel runat="server" CssClass="col-md-12 fieldset partidas__fraccion-contenedor flex-column">

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 partidas__fraccion" Type="Text" ID="ic_FraccionArancelaria" Label="Fracción" />

                                    <span class="partidas__descripción pl-5">Azúcar refinada estándar</span>

                                </asp:Panel>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_Nico" Label="Subd/ Núm.Id. comercial." Format="Numeric" />

                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-6" ID="sc_PartidaVinculacion" Label="Vinculación" OnText="Si" OffText="No" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="sc_PartidaMetodoValoracion" Label="Método de valoración" KeyField="i_ClaveMetodoValoracion" DisplayField="t_DescripcionMetodoValoracion" Dimension="Vt022MetodosValoracionA11" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="sc_UMC" Label="UMC" KeyField="i_ClaveUnidadMedida" DisplayField="t_DescripcionUnidadMedida" Dimension="Vt022UnidadesMedidaA07" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_CantidadUMC" Label="Cantidad UMC" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="sc_UMT" Label="UMT" KeyField="i_ClaveUnidadMedida" DisplayField="t_DescripcionUnidadMedida" Dimension="Vt022UnidadesMedidaA07" />

                            </div>

                            <div class="col-xs-12 col-md-6 mb-4">

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_CantidadUMT" Label="Cantidad UMT" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="sc_PaisVC" Label="P. V/C" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="sc_PaisOD" Label="P. O/D" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaValorAduana" Label="Val. Adu/Val. USD" Format="Money" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaPrecioPagado" Label="Imp. precio pag/Val. Com." Format="Money" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaPrecioUnitario" Label="Precio unitario" Format="Money" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaValorAgregado" Label="Val. agreg." Format="Money" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaMarca" Label="Marca" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaModelo" Label="Modelo" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="ic_PartidaCodigoProducto" Label="Código de producto" />

                            </div>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5 solid-textarea" Type="TextArea" ID="ic_PartidaDescripcion" Label="Descripción ( renglones variables según se requirera)" />

                            <GWC:CatalogControl ID="catalogoPartidaTasas" runat="server" KeyField="Catalogo_partida_tasas" CssClass="row mb-5">

                                <Columns>

                                    <GWC:SelectControl runat="server" CssClass="col-md-12" ID="sc_PartidaContribucion" Label="Contribución" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_PartidaTipoTasa" Label="Cve. tipo tasa" KeyField="i_ClaveTipoTasa" DisplayField="t_DescripcionTipoTasa" Dimension="Vt022TiposTasasA18" />

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_PartidaTasa" Label="Tasa" />
                                    <%--Format="Money"--%>
                                </Columns>

                            </GWC:CatalogControl>

                            <GWC:CatalogControl ID="CatalogoPartidasIdentificadores" runat="server" KeyField="Catalogo_partidas_identificadores" CssClass="row mb-5">

                                <Columns>

                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="sc_PartidaIdentificador" Label="Identificador" KeyField="t_Cve_Identificador" DisplayField="t_Identificador" Dimension="Vt022IdentificadoresA08" />

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_PartidaComplemento1" Label="Complemento 1" />

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_PartidaComplemento2" Label="Complemento 2" />

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="ic_PartidaComplemento3" Label="Complemento 3" />

                                </Columns>

                            </GWC:CatalogControl>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea" Type="TextArea" ID="ic_PartidaObservacion" Label="Observaciones a nivel partida" />

                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>

            </Fieldsets>

        </GWC:FormControl>


    </div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
