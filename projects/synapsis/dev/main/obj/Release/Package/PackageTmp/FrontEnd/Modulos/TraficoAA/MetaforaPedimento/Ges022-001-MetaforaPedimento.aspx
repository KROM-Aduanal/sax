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


        .partidas__descripción {
            color: #757575;
        }

        .partidas__fraccion-contenedor{
            border: 1px solid #e0e0e0;  
            padding: 30px 8px;
            border-radius: 15px;
            display: flex;
            position: relative;
            margin: 8px 0;
            /*font-size: 18px;*/
            margin-bottom: 24px !important;
        }
       
        .partidas__fraccion-contenedor > div {
            margin-top:12px;
        }
        .partidas_secuencia-partida {
            width: 68px;
            display: block;
            background-color:#6c4fd3;
            position:relative;
            border-radius: 50%;
        }
        .partidas_secuencia-partida:before {
            content: "";
            display:block;
            padding-top: 100%;
        }
        .partidas_secuencia-partida span {
            position: absolute;
            top:50%;
            left:50%;
            transform: translate(-50%,-50%);
            color:#ffffff;
            display: block;
            font-weight: bolder;
            font-size:18px;
        }
       
        .mxn-label {
            text-align: center;
            padding-top: 8px;
            font-weight: bold;
            opacity: .75;
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
                    <GWC:ButtonItem Text="Mandar por Correo" />
                    <GWC:ButtonItem Text="Generar partidas" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>

                <GWC:FieldsetControl runat="server" ID="fscDatosGenerales" Label="Generales">

                    <ListControls>

                        <%--CP_REFERENCIA--%>
                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="dbcReferenciaPedimento" Label="Referencia aduanal" LabelDetail="Número de pedimento" />

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 d-flex align-items-center mb-5">

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 pl-0" ID="scTipoReferencia" Label="Tipo referencia" OnSelectedIndexChanged="sc_TipoReferencia_SelectedIndexChanged">
                                <Options>
                                    <GWC:SelectOption Value="1" Text="Operativa" />
                                    <GWC:SelectOption Value="2" Text="Corresponsalía" />
                                    <GWC:SelectOption Value="3" Text="Corresponsalía terceros" />
                                </Options>
                            </GWC:SelectControl>

                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 pr-0" ID="scPrefijoReferencia" Label="Prefijo" OnSelectedIndexChanged="sc_PrefijoReferencia_SelectedIndexChanged" />

                        </asp:Panel>

                        <GWC:SelectControl runat="server" CssClass="CP_MODALIDAD_ADUANA_PATENTE col-xs-12 col-md-6 mb-5" ID="scPatente" Label="Patente" OnClick="sc_Patente_Click">                            
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" ID="scEjecutivoCuenta" Label="Ejecutivo de cuenta" KeyField ="i_Cve_EjecutivosMisEmpresas" DisplayField ="t_NombreCompleto" Dimension ="EjecutivosMiEmpresa">                           
                        </GWC:SelectControl>

                        <%--CA_CVE_TIPO_OPERACION--%>
                        <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 d-flex align-items-center jc-center" ID="swcTipoOperacion" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true" OnCheckedChanged="swcTipoOperacion_CheckedChanged"/>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scClavePedimento" Label="Clave Pedimento" KeyField="i_Cve_ClavePedimento" DisplayField="t_ClaveDescripcion" Dimension="Vt022ClavesPedimentoA02" OnSelectedIndexChanged="scClavePedimento_SelectedIndexChanged"/>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-6" ID="scRegimen" Label="Régimen" KeyField="t_Cve_Regimen" DisplayField="t_DescripcionCorta" Dimension="Vt022RegimenesA16" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scDestinoMercancia" Label="Destino de la mercancia" KeyField="i_ClaveDestinoMercancia" DisplayField="t_DescripcionDestinoMercancia" Dimension="Vt022DestinosMercanciasA15" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icTipoCambio" Locked="true" Label="Tipo de cambio" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icPesoBruto" Label="Peso bruto" Rules="onlynumber" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scAduanaEntradaSalida" Label="Aduana de entrada/salida" Enabled="false" KeyField="t_Cve_Aduana" DisplayField="t_ClaveDescripcion" Dimension="Vt022AduanaSeccionA01" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scTransporteEntradaSalida" Label="Transporte entrada/salida" KeyField="t_Cve_MedioTransporte" DisplayField="t_ClaveDescripcion" Dimension="Vt022MediosTransporteA03" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scMedioTransporteArribo" Label="Medio de transporte/arribo" Enabled="false" KeyField="t_Cve_MedioTransporte" DisplayField="t_ClaveDescripcion" Dimension="Vt022MediosTransporteA03" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scMedioTransporteSalida" Label="Medio de transporte /salida" KeyField="t_Cve_MedioTransporte" DisplayField="t_ClaveDescripcion" Dimension="Vt022MediosTransporteA03" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icValorDolares" Label="Valor en dólares" Format="Money" Enabled="false" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icValorAduana" Label="Valor aduana" Enabled="false" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icPrecioPagado" Label="Precio pagado/Valor comercial" Enabled="false" />

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscDatosImportador" Detail="Datos Importador" Label="Datos Importador">

                    <ListControls>
                        
                        <asp:Panel runat="server" CssClass="col-md-12 p-0">
                           
                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" ID="fbcCliente" Label ="Cliente" KeyField ="_id" DisplayField ="CA_RAZON_SOCIAL" OnTextChanged="fbc_Cliente_TextChanged" OnClick="fbc_Cliente_Click" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icRFCCliente" Label="RFC" Enabled="false" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icCURP" Label="CURP" Enabled="false" />

                            </div>

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icRFCFacturacion" Label="RFC facturación" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="TextArea" ID="icDomicilioCliente" Label="Domicilio" Enabled="false" />

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
                        
                        
                        <asp:Panel runat="server" CssClass="decrementable-box col-xs-12 mb-5" ID="pcIncrementables">
                        
                            <asp:Panel runat="server" CssClass="row fieldset " fieldset-legend="Valor incrementables">
                            
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="icValorSeguros" Label="Valor seguros" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="icSeguros" Label="Seguros dec." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="icFletes" Label="Fletes" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icEmbalajes" Label="Embalajes" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icOtrosIncrementables" Label="Otros incrementables" />

                            
                            </asp:Panel>

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="decrementable-box col-xs-12 mb-5" ID="pcDecrementables">

                            <asp:Panel runat="server" CssClass="row fieldset " fieldset-legend="Valor decrementables">

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="icTransporteDec" Label="Transporte dec." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="icSegurosDec" Label="Seguro dec." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="icCargaDec" Label="Carga" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3" Type="Text" ID="icDescargaDec" Label="Descarga" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3" Type="Text" ID="icOtrosDec" Label="Otros decrementables" />

                            </asp:Panel>

                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscValidacionPago" Detail="Validación y pago" Label="Validación" Enabled="false">

                    <ListControls>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scValidadorDesignado" Label="Validador asignado">
                            <Options>
                                <GWC:SelectOption Text="Seleccionar" Value="" />
                                <GWC:SelectOption Text="algo" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scNumeroSemana" Label="Número de semana">
                            <Options>
                                <GWC:SelectOption Text="Seleccionar" Value="0" />
                                <GWC:SelectOption Text="algo" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icArchivoValidacion" Label="Archivo de validación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icAcuseValidación" Label="Respuest de validación/Acuse electrónico de validación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icArchivoPago" Label="Archivo de pago" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icAcusetaPago" Label="Respuesta de pago/Acuse electrónico de pago" />

                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scValidacionAduanaDespacho" Label="Aduana de despacho">
                            <Options>
                                <GWC:SelectOption Text="Seleccionar" Value="0" />
                                <GWC:SelectOption Text="430 - Vercruz, Ver, México" Value="1" />
                            </Options>
                        </GWC:SelectControl>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 nopadding">

                            <label class="info-label pl-5">Código de barra</label>

                            <GWC:ImageControl CssClass="col-xs-12" runat="server" Source="/FrontEnd/Recursos/Imgs/barcode.jpg" Height="60px" Width="350px" Aspect="Cover" />

                        </asp:Panel>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5 d-flex justify-content-center align-items-center" Type="Text" ID="icMarcasNumeros" Label="Marcas, Números y total de bultos" />

                        <%--ESTOS CAMPOS NO APARECEN EN LA METAFORA DEL mockflow--%>
                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_CLAVE_SAD" Label="Sección aduanera de despacho" />--%>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icCertificacion" Label="Certificación" />

                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_AÑO_VALIDACION_2" Label="Año de validación 2" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_ADUANA_SIN_SECCION" Label="Aduana sin sección" />--%>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaValidacion" Label="Fecha de Validación" />

                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CP_MODALIDAD" Label="Modalidad" />--%>
                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Fechas" Detail="Fechas" Label="Fechas">

                    <ListControls>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaRegistro" Label="Registro" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaRevalidacion" Label="Revalidación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaZarpe" Label="Zarpe" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaPrevio" Label="Previo" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaFondeo" Label="Fondeo" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaPago" Label="pago de pedimento" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaAtraque" Label="Atraque" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaDespacho" Label="Despacho" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaEstimadaArribo" Label="Estimada de arribo" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaEntrega" Label="Entrega" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaEntrada" Label="Entrada" OnTextChanged="icFecha_TextChanged"/>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaPresentacion" Label="Presentación" OnTextChanged="icFecha_TextChanged" Visible="false"/>


                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaFacturacion" Label="Facturación" />

                        <%--ESTOS CAMPOS NO APARECEN EN LA METAFORA DEL mockflow--%>
                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_EXTRACCION" Label="Extracción" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_PRESENTACION" Label="Presentación" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_IMP_EUA_CAN" Label="IMP EUA CAN" />

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="CA_FECHA_ORIGINAL" Label="Original" />--%>
                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscTasasContribuciones" Detail="Tasas y totales" Label="Tasas y totales">

                    <ListControls>

                        <GWC:CatalogControl CssClass="w-100" ID="ccTasas" runat="server" KeyField="Catalogo_tasas" UserInteraction="false">

                            <Columns>

                                <GWC:SelectControl runat="server" ID="scTasasContribucion" Label="Contribución" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:SelectControl runat="server" ID="scTasasTipoTasa" Label="Cve. tipo tasa" KeyField="i_ClaveTipoTasa" DisplayField="t_DescripcionTipoTasa" Dimension="Vt022TiposTasasA18" />

                                <GWC:InputControl runat="server" Type="Text" ID="icTasasTasa" Label="Tasa" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscCuadroLiquidacion" Detail="Cuadro de liquidación" Label="Cuadro de liquidación">

                    <ListControls>

                        <GWC:CatalogControl ID="ccCuadroLiquidacion" runat="server" KeyField="Catalogo_liquidacion" CssClass="w-100 bold" UserInteraction="false">

                            <Columns>

                                <GWC:SelectControl runat="server" ID="scCuadroLiquidacionConcepto" Label="Concepto" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:InputControl runat="server" Type="Text" ID="icCuadroLiquidacionDescripcion" Enabled="false" Label="Descripción" />

                                <GWC:SelectControl runat="server" ID="scCuadroLiquidacionFP" Label="Forma de pago" KeyField="i_ClaveFormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                <GWC:InputControl runat="server" Type="Text" ID="icCuadroLiquidacionImporte" Label="Importe" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                        <asp:Panel runat="server" CssClass="col-md-12 mb-5 nopadding cl_totales">

                            <div class="col-xs-12 col-md-3">
                                <div class="cl_totales__Total">
                                    Total <span class="cl_totales__moneda">MXN</span>
                                </div>
                            </div>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 cl_totales__montos" Type="Text" ID="icCuadroLiquidacionEfectivo" Format="Money" Label="Efectivo" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 cl_totales__montos" Type="Text" ID="icCuadroLiquidacionOtros" Format="Money" Label="Otros" />

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 cl_totales__montos" Type="Text" ID="icCuadroLiquidacionTotal" Format="Money" Label="Total" />

                        </asp:Panel>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscCetificacion" Detail="Certificación" Label="Certificación" Visible="false">

                    <ListControls>

                        <GWC:FieldsetControl runat="server" CssClass="linea_captura" ID="fscLineaCaptura" Detail="Depósito referenciado - línea de captura - información del pago" Label="Línea de captura">

                            <ListControls>

                                <asp:Panel runat="server" CssClass="col-md-12">

                                    <div class="col-xs-12 col-md-6">

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 nopadding">

                                            <label class="info-label pl-5">Código de barra</label>

                                            <GWC:ImageControl CssClass="col-xs-12" runat="server" Source="/FrontEnd/Recursos/Imgs/barcode.jpg" Height="60px" Width="350px" Aspect="Cover" />

                                        </asp:Panel>

                                        <h3 class="w-100 d-flex py-3 justify-content-center">**Pago electrónico**</h3>

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="icLineaCapturaPatente" Label="Patente" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="icLineaCapturaPedimento" Label="Pedimento" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="icLineaCapturaAduana" Label="Aduana" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaBanco" Label="Banco" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaNumero" Label="Línea de captura" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaImporte" Format="Money" Label="Importe pagado" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaPago" Format="Calendar" Label="Fecha de pago" />

                                    </div>

                                    <div class="col-xs-12 col-md-6">

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaNumeroOperacion" Label="Número de operación bancaria" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaNumeroTransaccion" Label="Número de transacción al SAT" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaPresentacion" Label="Medio de presentación" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icLineaCapturaRecepcion" Label="Medio de recepción/cobro" />

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 linea_captura__rqcode">

                                            <GWC:ImageControl CssClass="col-xs-12" runat="server" Source="/FrontEnd/Recursos/Imgs/rqcode.png" Height="200px" Width="200px" Aspect="Cover" />

                                        </asp:Panel>

                                    </div>

                                </asp:Panel>

                            </ListControls>

                        </GWC:FieldsetControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscDatosProveedoresImpo" Detail="Proveedores" Label="Datos de proveedores">

                    <ListControls>

                        
                            <GWC:PillboxControl runat="server" ID="pbcProveedores" KeyField="i_Cve_PbProveedores" CssClass="w-100">

                                <ListControls>

                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 p-0">

                                        <GWC:FindboxControl runat="server" CssClass="col-md-12 mb-5" ID="fbxProveedor" Label="Nombre, denominación, razon social" KeyField="_id" DisplayField="razonsocial" OnTextChanged="fbxProveedor_TextChanged"/>

                                        <GWC:InputControl runat="server" CssClass="col-md-12 mt-4 mb-5" Type="Text" ID="icIdFiscalProveedor" Label="ID Fiscal" Enabled="false" />

                                    </asp:Panel>

                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 p-0">

                                        <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="TextArea" ID="icDocimilioProveedor" Label="Domicilio" Enabled="false" />

                                    </asp:Panel>
                                    
                                    <GWC:FieldsetControl runat="server" Label="Facturas" Priority="false">
                                        <ListControls>

                                            <GWC:CatalogControl ID="ccFacturas" runat="server" KeyField="Catalogo_facturas" CssClass="w-100 mt-5 mb-5 p-0" OnRowChanged="ccFacturas_RowChanged">

                                            <Columns>

                                                <GWC:SelectControl runat="server" ID="scFacturaProveedor" Label="Factura" OnClick="scFacturaProveedor_Click"/>

                                                <GWC:InputControl runat="server" Type="Text" ID="icFechaFacturaProveedor" Label="Fecha" Format="Calendar" Enabled="false"/>

                                                <GWC:InputControl runat="server" Type="Text" ID="icIncontermProveedor" Label="Inconterm" Enabled="false"/>

                                                <GWC:SelectControl runat="server" ID="scFactorMonedaProveedor" Label="Factor moneda" Enabled="false"/>

                                                <GWC:InputControl runat="server" Type="Text" ID="icMontoFacturaProveedor" Label="Monto" Format="Money" Enabled="false"/>

                                                <GWC:InputControl runat="server" Type="Text" ID="icMontoFacturaUSDProveedor" Label="Monto USD" Format="Money" Enabled="false"/>

                                            </Columns>

                                        </GWC:CatalogControl>

                                        </ListControls>
                                    </GWC:FieldsetControl>

                                </ListControls>

                            </GWC:PillboxControl>

                                

                           

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
                       

                        <%--<GWC:CatalogControl ID="ccProveedores" runat="server" KeyField="Catalogo_proveedor" CssClass="col-md-12 mt-5 p-0">

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

                <%--<GWC:FieldsetControl runat="server" ID="fscDatosProveedoresExpo" Detail="Datos proveedores / exportación" Label="Datos de proveedores expo">

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

                <%--<GWC:FieldsetControl runat="server" ID="fscDatosProveedores" Detail="Datos proveedores" Label="Datos de proveedores">

                    <ListControls>

                        <GWC:CatalogControl ID="ccProveedores" runat="server" KeyField="Catalogo_proveedor">

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

                <GWC:FieldsetControl runat="server" ID="fscDestinatarios" Detail="Datos destinatarios" Label="Destinatarios">

                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-md-12 p-0">

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="Text" ID="icTaxtIDDestinatario" Label="Taxt ID" Enabled="false" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="scRazonSocialDestinatario" Label="Destinatario">
                                    <Options>
                                        <GWC:SelectOption Text="Samsung electronics S.A. DE C.V." Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                            </div>

                            <div class="col-xs-12 col-md-6 p-0">

                                <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="TextArea" ID="icDomicilioDestinatario" Label="Domicilio" Enabled="false" />

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

                <%--<GWC:FieldsetControl runat="server" ID="fscDestinatarios" Detail="Destinatarios" Label="Destinatarios">

                    <ListControls>

                        <GWC:CatalogControl ID="ccDestinatarios" runat="server" KeyField="Catalogo_destinatarios">

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

                <GWC:FieldsetControl runat="server" ID="fscDatosTransporte" Detail="Datos del transporte" Label="Transporte">

                    <ListControls>

                        <GWC:PillboxControl runat="server" ID="pbcDatosTransporte" CssClass="col-md-12" KeyField="i_Cve_PbTransporte" >

                            <ListControls>

                                <asp:Panel runat="server" CssClass="col-md-12 p-0 py-5 mt-0">

                                    <div class="col-xs-12 col-md-6 mt-5 p-0">

                                        <GWC:InputControl runat="server" CssClass="col-md-12 mb-5" Type="Text" ID="icIDTransporte" Label="Identificación" />

                                        <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="scPaisTransporte" Label="País" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                        <GWC:SelectControl runat="server" CssClass="col-md-12 mb-5" ID="scTransportista" Label="Transportista">
                                            <Options>
                                                <GWC:SelectOption Text="Auto Express Hércules, S.A. de C.V." Value="1" />
                                                <GWC:SelectOption Text="Fletes Sotelo, S.A. de C.V." Value="1" />
                                            </Options>
                                        </GWC:SelectControl>

                                    </div>
                                    <div class="col-xs-12 col-md-6 mt-5 p-0">

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 mb-5" Type="Text" ID="icTransportistaRfc" Label="RFC" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 mb-5" Type="Text" ID="icTransportistaCurp" Label="CURP" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 mb-5" Type="Text" ID="icTransportistaDomicilio" Label="Domicilio" />

                                    </div>

                                </asp:Panel>

                            </ListControls>

                        </GWC:PillboxControl>

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

                <GWC:FieldsetControl runat="server" ID="fsDatosCandados" Detail="Candados" Label="Candados">
                    <ListControls>

                        <GWC:CatalogControl ID="ccCandados" runat="server" KeyField="Catalogo_candados" CssClass="w-100">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="icNumeroCandado" Label="Candado" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="icCandadoPrimeraRevisión" Label="Primera revisión" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="icCandadoSegundaRevision" Label="Segunda revisión" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>
                    
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscGuias" Detail="Guias, manifiestos / conocimientos de embarque" Label="Guías">
                   
                    <ListControls>

                        <GWC:CatalogControl ID="ccGuias" runat="server" KeyField="Catalogo_guias" CssClass="w-100">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icGuia" Label="Guía, manifiestos o BL" />

                                <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="CA_MASTER_O_HOUSE" Label="Guía maester o house" />--%>

                                <%--<GWC:SelectControl runat="server" CssClass="col-md-12 mt-5" ID="" Label="Número de guía | conocimiento embarque">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>--%>

                                <GWC:SwitchControl runat="server" CssClass="col-md-12 mt-5 d-flex align-items-center jc-center" ID="swcTipoGuia" Label="ID (Tipo de guía) " OnText="Master" OffText="House" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscContenedores" Detail="Contenedores" Label="Contenedores">

                    <ListControls>

                        <GWC:CatalogControl ID="ccContenedores" runat="server" KeyField="Catalogo_contenedores" CssClass="w-100">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icNumeroContenedor" Label="Número  de cont | Equipo ferrocarril | Núm. económico" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scTipoContenedor" Label="Tipo" KeyField="i_ClaveTipoContenedorVehiculoTransporte" DisplayField="t_DescripcionTipoContenedorVehiculoTransporte" Dimension="Vt022TiposContenedoresVehiculosTransporteA10" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <%--<GWC:FieldsetControl runat="server" ID="fscCandados" Detail="Candados" Label="Candados">

                    <ListControls>

                        <GWC:CatalogControl ID="ccCandados" runat="server" KeyField="Catalogo_candados">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUM_CANDADO" Label="Número candado" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUM_CANDADO_1RA" Label="Primera revisión" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="CA_NUM_CANDADO_2DA" Label="Segunda revisión" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>--%>

                <%--<GWC:FieldsetControl runat="server" ID="fscFacturas" Detail="Facturas" Label="Facturas">

                    <ListControls>

                        <GWC:CatalogControl ID="ccFacturas" runat="server" KeyField="Catalogo_facturas">

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

                <GWC:FieldsetControl runat="server" ID="fscIdentificadores" Detail="Identificadores" Label="Identificadores">

                    <ListControls>

                        <GWC:CatalogControl ID="ccIdentificadores" runat="server" KeyField="Catalogo_identificadores" CssClass="w-100">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scIdentificadorPedimento" Label="Clave" KeyField="t_Cve_Identificador" DisplayField="t_Identificador" Dimension="Vt022IdentificadoresA08" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icComplemento1Pedimento" Label="Complemento 1" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icComplemento2Pedimento" Label="Complemento 2" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icComplemento3Pedimento" Label="Complemento 3" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscCuentasAduaneras" Detail="Cuentas aduaneras" Label="Cuentas aduaneras">

                    <ListControls>

                        <GWC:CatalogControl ID="ccCuentasAduaneras" runat="server" KeyField="Catalogo_cuentasAduaneras" CssClass="w-100">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scCuentaAduanera" Label="Cuenta aduanera">
                                    <Options>
                                        <GWC:SelectOption Text="Cuenta aduanera de garantía global" Value="2" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scTipoCuentaAduanera" Label="Tipo garantia">
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

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scInstitucionEmisora" Label="Emisora">
                                    <Options>
                                        <GWC:SelectOption Text="BBVA México, S.A., Institución de Banca Múltiple, Grupo Financiero BBVA México." Value="1" />
                                        <GWC:SelectOption Text="Banco Nacional de México, S.A." Value="2" />
                                        <GWC:SelectOption Text="Banco HSBC, S.A. de C.V." Value="3" />
                                        <GWC:SelectOption Text="Bursamex, S.A. de C.V." Value="4" />
                                        <GWC:SelectOption Text="Operadora de Bolsa, S.A. de C.V." Value="5" />
                                        <GWC:SelectOption Text="Vector Casa de Bolsa, S.A. de C.V." Value="6" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icNumeroCOntratoCuentaAduanera" Label="# contrato 3" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icFolioConstanciaCuentaAduanera" Label="Folio" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icImporteCuentaAduanera" Label="Importe" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icFechaEmisionCuentaAduanera" Label="Emisión" Format="Calendar" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icPrecioEstimadoCuentaAduanera" Label="Precio estimado" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icTitulosCuentaAduanera" Label="Titulos" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icValorUnitario" Label="Valor unitario" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscPagosvirtuales" Detail="Formas de pago virtual" Label="Pago virtual">

                    <ListControls>

                        <GWC:CatalogControl ID="ccPagosvirtuales" runat="server" KeyField="Catalogo_pagosvirtuales" CssClass="w-100">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scPagosVirtualesFormaPago" Label="Forma de pago" KeyField="i_ClaveFormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scPagosVIrtualesEmisora" Label="Emisora">
                                    <Options>
                                        <GWC:SelectOption Text="Seleccionar" Value="0" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icPagosVirtualesDocumento" Label="# documento" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icPagosVirtualesFechaDocumento" Label="Fecha doc." Format="Calendar" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icPagosVirtualesImporteDocumento" Label="Importe" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icPagosVirtualesSaldo" Label="Saldo" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icPagosVirtualesImportePedimento" Label="Importe" Format="Numeric" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscRectificaciones" Detail="Rectificaciones" Label="Rectificaciones" CssClass="col-md-12" Visible="false">

                    <ListControls>

                        <GWC:CatalogControl ID="ccRectificaciones" runat="server" KeyField="Catalogo_rectificaciones" UserInteraction="false" CssClass="w-100">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icRectificacionesFechaPedOriginal" Label="Fecha ped. ori." Format="Calendar" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scRectificacionesClavePedOriginal" Label="Clave Ped. ori." KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icRectificacionesPatentePedOriginal" Label="Patente ori." />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icRectificacionesPedimentoCompleto" Label="Pedimento ori." />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icRectificacionesAñoValidacion2PedOriginal" Label="Año val. ori. 2" />
                                <%--Format="Numeric"--%>

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icRectificacionesAñoValidacionPedOriginal" Label="Año val. ori." />
                                <%--Format="Numeric"--%>

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scRectificacionesAduanaOriginal" Label="Aduana des. ori." KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scRectificacionesAduana2Original" Label="Aduana des. ori. 2" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scRectificacionesClavePedimento" Label="Clave Ped. recti." KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icRectificacionesFechaPedimento" Label="Fecha ped. recti." />
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

                <GWC:FieldsetControl runat="server" ID="fscDiferenciaContribuciones" Detail="Diferencias de contribuciones" Label="Dif. contribuciones" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="ccDiferenciasRecti" runat="server" KeyField="Catalogo_DiferenciasRecti" UserInteraction="false" CssClass="w-100">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDifConConcepto" Label="Concepto" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDifConFormaPago" Label="Forma de pago" KeyField="i_ClaveFormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDifConDiferencia" Label="Diferencia" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDifConEfectivo" Label="Efectivo" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDifConOtros" Label="Otros" />
                                <%--Format="Money"--%>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDifConTotal" Label="Dif totales" />
                                <%--Format="Money"--%>
                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscDescargos" Detail="Descargos" Label="Descargos" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="ccDescargos" runat="server" KeyField="Catalogo_descargos" CssClass="w-100">

                            <Columns>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosPedCompletoOriginal" Label="Pedimento" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosFechaPedOriginal" Label="Original" Format="Calendar" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDescargosClavePedOriginal" Label="Clave" KeyField="i_Cve_ClavePedimento" DisplayField="t_Cve_Pedimento" Dimension="Vt022ClavesPedimentoA02" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosValidacionOriginal" Label="Año val. original" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosValidacion2Original" Label="Año val. original 2" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDescargosPatenteOriginal" Label="Patente">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="1" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDescargosAduanaOriginal" Label="Aduana des. ori." KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDescargosAduana2Original" Label="Aduana des. ori. 2" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosPedOriginal" Label="Pedimento ori. 2" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosFraccionOriginal" Label="Fraccion original" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosUMOriginal" Label="UM original" Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icDescargosUMDescargo" Label="UM descargo" Format="Numeric" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscCompensaciones" Detail="Compensaciones" Label="Compensaciones" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="ccCompensaciones" runat="server" KeyField="Catalogo_compensaciones" CssClass="w-100">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scCompensacionesContribucion" Label="Contribución" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCompensacionesPedCompletoOriginal" Label="Pedimento ori. com." />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCompensacionesFechaPagoPedOriginal" Label="Pago original" Format="Calendar" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCompensacionesGravamen" Label="Gravamen" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scCompensacionesConcepto" Label="Concepto">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="0" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCompensacionesAñoValidacionOriginal" Label="Año val. ori." Format="Numeric" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCompensacionesAñoValidacion2Original" Label="Año val. ori. 2" Format="Numeric" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scCompensacionesPatenteOriginal" Label="Patente original">
                                    <Options>
                                        <GWC:SelectOption Text="Selecciona" Value="0" />
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scCompensacionesAduanaOriginal" Label="Aduana des. ori." KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scCompensacionesAduana2Original" Label="Aduana des. ori. 2" KeyField="t_Cve_Aduana" DisplayField="t_AduanaSeccionDenominacion" Dimension="Vt022AduanaSeccionA01" />

                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCompensacionesPedOriginal" Label="Pedimento ori. 2" />

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscPruebaSuficiente" Detail="Prueba suficiente" Label="Prueba suficiente" CssClass="col-md-12">

                    <ListControls>

                        <GWC:CatalogControl ID="ccPruebaSuficiente" runat="server" KeyField="Catalogo_pruebaSuficiente" CssClass="w-100">

                            <Columns>

                                <GWC:SelectControl runat="server" CssClass="col-md-12" ID="scPruebaSuficientePaisDestino" Label="País destino" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                <GWC:InputControl runat="server" CssClass="col-md-12" Type="Text" ID="icPruebaSuficientePedEUACAN" Label="# ped. EUA/CAN." />

                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scPruebaSuficiente" Label="Pru. suficiente">
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

                <GWC:FieldsetControl runat="server" ID="fscObservaciones" Detail="Observaciones / a nivel pedimento" Label="Observaciones ped.">

                    <ListControls>

                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea" Type="TextArea" ID="icOnservacionesPedimento" Label="Observaciones" />

                    </ListControls>

                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscPartidas" Detail="Partidas" Label="Partidas">

                    <ListControls>

                        <GWC:PillboxControl runat="server" CssClass="w-100" ID="pbcPartidas" KeyField="indice" OnCheckedChange="pbcPartidas_CheckedChange" OnClick="pbcPartidas_Click">
                            <ListControls>

                                <asp:Panel runat="server" CssClass="row no-gutters">

                                    <asp:Panel runat="server" class="col-xs-12 col-md-6">

                                        <asp:Panel runat="server" CssClass="partidas__fraccion-contenedor">

                                            <asp:Label runat="server" CssClass="partidas_secuencia-partida">
                                                <asp:Label runat="server" ID="lbSecuencia" Text="1"></asp:Label>
                                            </asp:Label>
                                            
                                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-7" ID="icFraccionArancelaria" Label="Fracción" OnTextChanged="icFraccionArancelaria_TextChanged" OnClick="icFraccionArancelaria_Click"/>

                                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-4" Type="Text" ID="icNico" Label="Nico"/>

                                        </asp:Panel>

                                        <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3" Type="Text" ID="icPartidaCodigoProducto" Label="Código de producto" />--%>

                                        <GWC:InputControl runat="server" CssClass="col-xs-11 col-md-5 pr-0 mb-5" Type="Text" ID="icPartidaPrecioUnitario" Label="Precio unitario" Format="Money" />

                                        <asp:Label runat="server" Text="MXN" CssClass="col-xs-1 col-md-1 pl-0 mxn-label mb-5"></asp:Label>


                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPartidaMetodoValoracion" Label="Método de valoración" KeyField="i_ClaveMetodoValoracion" DisplayField="t_DescripcionMetodoValoracion" Dimension="Vt022MetodosValoracionA11" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icCantidadUMC" Label="Cantidad UMC" Format="Numeric" />

                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scUMC" Label="UMC" KeyField="i_ClaveUnidadMedida" DisplayField="t_DescripcionUnidadMedida" Dimension="Vt022UnidadesMedidaA07" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icCantidadUMT" Label="Cantidad UMT" Format="Numeric" />
                                        
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scUMT" Label="UMT" KeyField="i_ClaveUnidadMedida" DisplayField="t_DescripcionUnidadMedida" Dimension="Vt022UnidadesMedidaA07" />

                                    </asp:Panel>

                                    <asp:Panel runat="server" class="col-xs-12 col-md-6">

                                        <GWC:InputControl runat="server" CssClass="col-xs-11 col-md-5 pr-0 mb-5" Type="Text" ID="icPartidaValorAduana" Label="Valor Aduana" Format="Money" />
                                        <asp:Label runat="server" ID="lbValorAduana" Text="MXN" CssClass="col-xs-1 col-md-1 pl-0 mxn-label mb-5"></asp:Label>
                                        <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-11 col-md-5 pr-0 mb-5" Type="Text" ID="icPartidaValorUSd" Label="Valor Dolares" Format="Money" />
                                        <asp:Label runat="server" ID="lbValorUsd" Visible="false" Text="USD" CssClass="col-xs-1 col-md-1 pl-0 mxn-label mb-5"></asp:Label>
                                        
                                       
                                        <GWC:InputControl runat="server" CssClass="col-xs-11 col-md-5 pr-0 mb-5" Type="Text" ID="icPartidaPrecioPagado" Label="Precio Pagado" Format="Money" />
                                        <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-11 col-md-5 pr-0 mb-5" Type="Text" ID="icPartidaValorComercial" Label="Valor Comercial" Format="Money" />
                                        <asp:Label runat="server" Text="MXN" CssClass="col-xs-1 col-md-1 pl-0 mxn-label mb-5"></asp:Label>

                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPaisVendedor" Label="Pais Vendedor" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />
                                        <GWC:FindboxControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" ID="scPaisComprador" Label="Pais Comprador" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />


                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPaisOrigen" Label="Pais Origen" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />
                                        <GWC:FindboxControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" ID="scPaisDestino" Label="Pais Destino" KeyField="t_ClaveSAAIM3" DisplayField="t_Pais" Dimension="Vt022PaisesA04" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icPartidaMarca" Label="Marca" />

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icPartidaModelo" Label="Modelo" />

                                        <%--<GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-6" ID="swcPartidaVinculacion" Label="Vinculación" OnText="Si" OffText="No" />--%>

                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPartidaVinculacion" Label="Vinculación">
                                            <Options>
                                                <GWC:SelectOption Value="1" Text="Si"/>
                                                <GWC:SelectOption Value="2" Text="Si pero"/>
                                                <GWC:SelectOption Value="3" Text="No"/>
                                            </Options>
                                        </GWC:SelectControl>

                                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icPartidaValorAgregado" Label="Valor Agregado" Format="Money" />
                                        
                                    </asp:Panel>

                                </asp:Panel>

                                <asp:Panel runat="server" CssClass="row no-gutters">

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5 solid-textarea" Type="TextArea" ID="icPartidaDescripcion" Label="Descripción ( renglones variables según se requirera)" />

                                    <GWC:FieldsetControl runat="server" Priority="false" Label="Contribuciones">
                                        <ListControls>

                                            <GWC:CatalogControl ID="ccPartidaTasas" runat="server" KeyField="indice" CssClass="w-100 mb-5">

                                                <Columns>

                                                    <GWC:SelectControl runat="server" ID="scPartidaContribucion" Label="Contribución" KeyField="i_ClaveContribucion" DisplayField="t_Abreviacion" Dimension="Vt022ContribucionesA12" />

                                                    <GWC:SelectControl runat="server" ID="scPartidaTipoTasa" Label="Cve. Tipo Tasa" KeyField="i_ClaveTipoTasa" DisplayField="t_DescripcionTipoTasa" Dimension="Vt022TiposTasasA18" />

                                                    <GWC:InputControl runat="server" Type="Text" ID="icPartidaTasa" Label="Tasa" />
                                   
                                                    <GWC:SelectControl runat="server" ID="scFormaPago" Label="Forma Pago" KeyField="i_Cve_FormaPago" DisplayField="t_DescripcionFormaPago" Dimension="Vt022FormasPagoA13" />

                                                    <GWC:InputControl runat="server" Type="Text" ID="icImporte" Label="Importe" />

                                                </Columns>

                                            </GWC:CatalogControl>

                                        </ListControls>
                                    </GWC:FieldsetControl>

                                    <GWC:FieldsetControl runat="server" Priority="false" Label="Permisos o Normas">
                                        <ListControls>

                                            <GWC:CatalogControl ID="ccPartidasPermisos" runat="server" KeyField="indice" CssClass="w-100 mb-5">

                                                <Columns>

                                                    <GWC:SelectControl runat="server" ID="scClavePermiso" Label="Clave Permiso" KeyField="i_Cve_RegulacionRestriccionNoArancelaria" DisplayField="t_Cve_RegulacionRestriccionNoArancelaria" Dimension="Vt022RegulacionesRestriccionesNoArancelariasA09" />

                                                    <GWC:InputControl runat="server" Type="Text" ID="icPermisoNom" Label="Num. Permiso o Nom" />
                                   
                                                    <GWC:InputControl runat="server" Type="Text" ID="icFirmaDescargo" Label="Firma Descargo" />

                                                    <GWC:InputControl runat="server" Type="Text" ID="icValorComercialDLS" Label="Val. Comercial DLS" />

                                                    <GWC:InputControl runat="server" Type="Text" ID="icCantidadUMTC" Label="Cantidad UMT/C" />

                                                </Columns>

                                            </GWC:CatalogControl>

                                        </ListControls>
                                    </GWC:FieldsetControl>

                                    <GWC:FieldsetControl runat="server" Priority="false" Label="Complementos">
                                        <ListControls>

                                            <GWC:CatalogControl ID="ccPartidasIdentificadores" runat="server" KeyField="indice" CssClass="w-100 mb-5">

                                                <Columns>

                                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scPartidaIdentificador" Label="Identificador" KeyField="t_Cve_Identificador" DisplayField="t_Identificador" Dimension="Vt022IdentificadoresA08" />

                                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icPartidaComplemento1" Label="Complemento 1" />

                                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icPartidaComplemento2" Label="Complemento 2" />

                                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icPartidaComplemento3" Label="Complemento 3" />

                                                </Columns>

                                            </GWC:CatalogControl>

                                        </ListControls>
                                    </GWC:FieldsetControl>

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea mt-4" Type="TextArea" ID="icPartidaObservacion" Label="Observaciones a nivel partida" />

                                </asp:Panel>
                        

                            </ListControls>
                        </GWC:PillboxControl>

                    </ListControls>

                </GWC:FieldsetControl>

            </Fieldsets>

        </GWC:FormControl>


    </div>

    <script>
            
        const section = document.querySelector('section-id="fscDatosImportador"');




    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
