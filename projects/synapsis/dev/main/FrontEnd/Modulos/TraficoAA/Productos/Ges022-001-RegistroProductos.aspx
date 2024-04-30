<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-RegistroProductos.aspx.vb" Inherits=".Ges022_001_RegistroProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

<% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Producto" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>

 <% End If %>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

<GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <style>

        .img-section {
            width: 90%;
            max-width:400px;
            position:relative;
            border: 2px dashed rgba(0,0,0,.15);
        }

        .img-section:before {
            content:"";
            display:block;
            padding-top:60%;
        }

        .img-section:after {
            content:"Subir Imagen";
            position:absolute;
            top:50%;
            left:50%;
            transform:translate(-50%,-50%);
            font-size: 18px;
            text-align:center;
            color:#000;
        }

        .mt-40 label {
           margin-top: 40px;
        }

        .breakline {
            display:block;
            margin: 20px 0;
            width: 100%
        }

        .custom_cat_3 table tr th:nth-child(2){
            width:70px;
        }
        .custom_cat_3 tr th:nth-child(7),.custom_cat_3 tr th:nth-child(8){
            width:205px;
        }
        .custom_cat_3 table tr th:nth-child(5){
            width:140px;
        }
        .btnAction {
            background: #43189a;
            height:36px;
            padding: 0 24px;
            border:0;
            outline:0;
            background-position:center;
            background-repeat:no-repeat;
            background-size: 24px;
            box-shadow: 0 1px 4px rgba(0,0,0,0.25);
        }
        .btnAction:disabled {
            background-color:#c4c0c0;
        }
        .btnArchivar {
            border-radius: 18px 0 0 18px;
            background-image: url("../../../Librerias/Krom/imgs/archivar.png");
        }
        .btnRestaurar {
            border-radius: 0 18px 18px 0;
            background-image: url("../../../Librerias/Krom/imgs/w_reply.png");
            margin-left:1px;
        }
        .btnSubmit {
            color: #ffffff;
            border-radius: 18px;
        }
        .custom_cat_3_content {
            border-top: 0;
            border-radius: 0 0 15px 15px;
            margin-top: -28px;
        }
        .wc-pillbox .fieldset {
            border-bottom: 0 !important;
            border-radius: 15px 15px 0 0 !important;
        }
        }
    </style>
   
    <div class="d-flex">
           <GWC:FormControl HasAutoSave="false" ID="__SYSTEM_MODULE_FORM" runat="server" Label="Catálogo de Productos" OnCheckedChanged="MarcarPagina">
                    <Buttonbar runat="server" OnClick="EventosBotonera">
                        <DropdownButtons>
                            <GWC:ButtonItem Text="Descargar"/>
                            <GWC:ButtonItem Text="Imprimir"/>
                            <GWC:ButtonItem Text="Mandar por Correo"/>
                        </DropdownButtons>
                    </Buttonbar>           
                    <Fieldsets>
                        <GWC:FieldsetControl ID="fscDatosGenerales" runat="server" Label="Datos Generales">
                            <ListControls>
                                <GWC:InputControl runat="server" ID="icNombreComercial" CssClass="col-xs-12 col-md-5" Label="Nombre comercial" Type="TextArea"/>
                                <GWC:SwitchControl runat="server" ID="swcEstadoProducto" CssClass="col-xs-12 col-md-1 mt-40" OnText="Si" OffText="No" Label="Habilitado"/>
                                
                                <GWC:FileControl runat="server" Dragable="true" Label="Imagen del producto" CssClass="col-xs-12 col-md-6"/>

                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl ID="fscClasificacion" runat="server" Label="Clasificación">
                            <ListControls>                                
                                <asp:Panel runat="server" CssClass="d-flex justify-content-end w-100">
                                    <asp:Button runat="server" ID="btnArchivar" CssClass="btnAction btnArchivar" OnClick="ConfigurarControlesClasificacion_Click"/>
                                    <asp:Button runat="server" ID="btnRestaurar" CssClass="btnAction btnRestaurar" OnClick="ConfigurarControlesClasificacion_Click"/>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="row fieldset">
                                    <GWC:FindboxControl runat="server" ID="fbcFraccionArancelaria" CssClass="col-xs-12 col-md-6 mb-3" Label="Fracción Arancelaria" OnTextChanged="fbx_FraccionArancelaria_TextChanged" OnClick="fbx_FraccionArancelaria_Click" RequiredSelect="true"/>
                                    <GWC:SelectControl runat="server" ID="scNico" CssClass="col-xs-12 col-md-6 mb-3" Label="Nico" OnSelectedIndexChanged="sl_Nico_Selected"></GWC:SelectControl>

                                    <GWC:InputControl runat="server" ID="icDescripcionFraccion" CssClass="col-xs-12 col-md-6 mb-5" Label="Descripción Fracción" Enabled="false" Type="TextArea"/>
                                    <GWC:InputControl runat="server" ID="icDescripcionNico" CssClass="col-xs-12 col-md-6 mb-5" Label="Descripción Nico" Enabled="false" Type="TextArea"/>

                                    <%-- <GWC:FindboxControl runat="server" ID="fbx_Nico" CssClass="col-xs-12 col-md-9 mb-5" Label="Nico" OnTextChanged="fbx_Nico_TextChanged"/> --%>
                                    <GWC:InputControl runat="server" ID="icFechaRegistro" CssClass="col-xs-12 col-md-6 mb-3" Label="Fecha de registro" Type="Text" Format="Calendar" Enabled="false"/>
                                    
                                    
                                    <GWC:SelectControl runat="server" ID="scEstatus" CssClass="col-xs-12 col-md-6 mb-3" Label="Estatus">
                                        <Options>
                                            <GWC:SelectOption Value="1" Text="Autorizado"/>
                                            <GWC:SelectOption Value="2" Text="Preliminar"/>
                                            <GWC:SelectOption Value="3" Text="Clasificado"/>
                                            <GWC:SelectOption Value="4" Text="Suprimido"/>
                                        </Options>
                                    </GWC:SelectControl>
                                    <GWC:InputControl runat="server" ID="icObservaciones" CssClass="col-xs-12 col-md-12 mb-3" Label="Observación" Type="TextArea"/>
                                    <GWC:InputControl runat="server" ID="icMotivo" CssClass="col-xs-12 col-md-12 mb-3" Label="Motivo" Type="TextArea" Visible="false"/>
                                    <asp:Panel runat="server" CssClass="d-flex justify-content-end w-100">
                                        <asp:Button runat="server" CssClass="btnAction btnSubmit" ID="btn_ConfirmarArchivado" Text="Archivar" Visible="false" OnClick="btn_ConfirmarArchivado_Click"/>
                                    </asp:Panel>
                                </asp:Panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl ID="fscDescripcionFacturas" runat="server" Detail="Descripciones en Facturas" Label="Descripciones">
                            <ListControls>
                                    <GWC:PillboxControl runat="server" CssClass="col-xs-12" ID="pbcDescipcionesFacturas" KeyField="indice">
                                        <ListControls>
                                            <%-- 
                                            <GWC:SwitchControl runat="server" ID="cd6" CssClass="col-xs-12 col-md-2 d-flex align-items-center justify-content-center" OnText="Si" OffText="No" Label="Combinar Descripción"/>
                                            --%>
                                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-4" ID="fbcCliente" Label="Cliente" OnTextChanged="fbx_Cliente_TextChanged" RequiredSelect="true"/>
                                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-4" ID="fbcProveedor" Label="Proveedor" OnTextChanged="fbx_Proveedor_TextChanged" RequiredSelect="true"/>                                   
                                       
                                            <GWC:FieldsetControl runat="server" Label="Descripciones" Priority="false" CssClass="mt-4">
                                                <ListControls>
                                                    <GWC:CatalogControl runat="server" ID="ccDescipcionesFacturas" CssClass="col custom_cat_3" KeyField="indice">
                                                        <Columns>
                                                            <GWC:InputControl Type="Text" runat="server" ID="icIdKrom" Label="Id Krom" Enabled="false"/>
                                                            <GWC:InputControl Type="Text" runat="server" ID="icNumeroParte" Label="Número de parte"/>
                                                            <GWC:InputControl runat="server" ID="icDescripcion" Label="Descripción" Type="Text"/>
                                                            <GWC:SwitchControl runat="server" ID="swcAplicaCove" OnText="Si" OffText="No" Label="Aplica COVE"/>
                                                            <GWC:InputControl runat="server" ID="icDescripcionCove" Label="Descripción COVE" Type="Text"/>
                                                            <GWC:InputControl Type="Text" runat="server" ID="icAlias" Label="Alias"/>
                                                            <GWC:SelectControl runat="server" ID="scTipoAlias" Label="Tipo Alias">
                                                                <Options>
                                                                    <GWC:SelectOption Value="1" Text="Modelo"/>
                                                                    <GWC:SelectOption Value="2" Text="Submodelo"/>
                                                                    <GWC:SelectOption Value="3" Text="Número de série"/>
                                                                    <GWC:SelectOption Value="4" Text="SKU"/>
                                                                    <GWC:SelectOption Value="5" Text="Código interno"/>
                                                                </Options>
                                                            </GWC:SelectControl>
                                                        </Columns>
                                                    </GWC:CatalogControl>
                                                </ListControls>
                                            </GWC:FieldsetControl>

                                        </ListControls>
                                    </GWC:PillboxControl>
                                    <asp:Panel runat="server" CssClass="w-100 fieldset custom_cat_3_content">
                                        
                                    </asp:Panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl ID="fscHistoriales" runat="server" Label="Historiales" Visible="false">
                            <ListControls>
                                <GWC:FieldsetControl ID="fscHistorialClasificacion" runat="server" Label="Historial de Clasificación" Priority="false">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccHistorialClasificacion" CssClass="w-100" KeyField="indice" UserInteraction="false">
                                            <Columns>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoFraccion" Label="Fracción Arancelaria"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoNico" Label="Fracción Nico"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoMotivo" Label="Motivo"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoFechaModificacion" Label="Fecha Modificacion"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl>
                                <GWC:FieldsetControl ID="fscHistorialDescipciones" runat="server" Label="Historial de Descripciones" Priority="false">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccHistorialDescripciones" CssClass="w-100" KeyField="indice" UserInteraction="false">
                                            <Columns>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoNumeroParte" Label="Número de parte"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoDescripcion" Label="Descripción"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoProveedor" Label="Proveedor"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="icHistoricoCliente" Label="Cliente"/>
                                                <%--<GWC:InputControl Type="Text" runat="server" ID="icHistoricoFechaArchivado" Label="Fecha archivado"/>--%>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl>
                            </ListControls>
                        </GWC:FieldsetControl>             
                    </Fieldsets>
           </GWC:FormControl>
     </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
    <script>
        $(() => {

            $("body").on("click","input[type='checkbox']", (e) => {

                const tr = e.target.closest(".__row");
               
                if (tr) {
                    
                    if (e.target.checked) {
                        
                        const desc = tr.querySelector("td:nth-child(4) input");
                        
                        const desccove = tr.querySelector("td:nth-child(6) input");
                        
                        desccove.value = desc.value;

                    }
                    
                }

            });

        });
    </script>
</asp:Content>
