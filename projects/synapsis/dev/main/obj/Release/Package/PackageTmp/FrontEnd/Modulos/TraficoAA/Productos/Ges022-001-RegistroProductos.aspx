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
                        <GWC:FieldsetControl ID="_fsdatosgenerales" runat="server" Label="Datos Generales">
                            <ListControls>
                                <GWC:InputControl runat="server" ID="txt_NombreComercial" CssClass="col-xs-12 col-md-5" Label="Nombre comercial" Type="TextArea"/>
                                <GWC:SwitchControl runat="server" ID="sw_EstadoProducto" CssClass="col-xs-12 col-md-1 mt-40" OnText="Si" OffText="No" Label="Habilitado"/>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 d-flex align-content-center justify-content-center">
                                    <div class="img-section"></div>
                                </asp:Panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl ID="_fsclasificacion" runat="server" Label="Clasificación">
                            <ListControls>                                
                                <asp:Panel runat="server" CssClass="d-flex justify-content-end w-100">
                                    <asp:Button runat="server" ID="btn_Archivar" CssClass="btnAction btnArchivar" OnClick="ConfigurarControlesClasificacion_Click"/>
                                    <asp:Button runat="server" ID="btn_Restaurar" CssClass="btnAction btnRestaurar" OnClick="ConfigurarControlesClasificacion_Click"/>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="row fieldset">
                                    <GWC:FindboxControl runat="server" ID="fbx_FraccionArancelaria" CssClass="col-xs-12 col-md-9 mb-5" Label="Fracción Arancelaria" OnTextChanged="fbx_FraccionArancelaria_TextChanged" OnClick="fbx_FraccionArancelaria_Click"/>
                                    <GWC:InputControl runat="server" ID="txtFechaRegistro" CssClass="col-xs-12 col-md-3 mb-3" Label="Fecha de registro" Type="Text" Format="Calendar"/>
                                    
                                    <%-- <GWC:FindboxControl runat="server" ID="fbx_Nico" CssClass="col-xs-12 col-md-9 mb-5" Label="Nico" OnTextChanged="fbx_Nico_TextChanged"/> --%>

                                    <GWC:SelectControl runat="server" ID="sl_Nico" CssClass="col-xs-12 col-md-9 mb-5" Label="Nico" OnClick="sl_Nico_Click"></GWC:SelectControl>

                                    <GWC:SelectControl runat="server" ID="sl_Estatus" CssClass="col-xs-12 col-md-3 mb-4" Label="Estatus">
                                        <Options>
                                            <GWC:SelectOption Value="1" Text="Autorizado"/>
                                            <GWC:SelectOption Value="2" Text="Preliminar"/>
                                            <GWC:SelectOption Value="3" Text="Clasificado"/>
                                            <GWC:SelectOption Value="4" Text="Suprimido"/>
                                        </Options>
                                    </GWC:SelectControl>
                                    <GWC:InputControl runat="server" ID="txt_Observaciones" CssClass="col-xs-12 col-md-12 mb-3" Label="Observación" Type="TextArea"/>
                                    <GWC:InputControl runat="server" ID="txt_Motivo" CssClass="col-xs-12 col-md-12 mb-3" Label="Motivo" Type="TextArea" Visible="false"/>
                                    <asp:Panel runat="server" CssClass="d-flex justify-content-end w-100">
                                        <asp:Button runat="server" CssClass="btnAction btnSubmit" ID="btn_ConfirmarArchivado" Text="Archivar" Visible="false" OnClick="btn_ConfirmarArchivado_Click"/>
                                    </asp:Panel>
                                </asp:Panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl ID="_fsdescripcionfacturas" runat="server" Detail="Descripciones en Facturas" Label="Descripciones">
                            <ListControls>
                                    <GWC:PillboxControl runat="server" CssClass="col-xs-12" ID="pbx_DescipcionesFacturas" KeyField="indice" OnBeforeClick="pbx_DescipcionesFacturas_BeforeClick" OnClick="pbx_DescipcionesFacturas_Click" OnCheckedChange="pbx_DescipcionesFacturas_CheckedChange">
                                        <ListControls>
                                            <%-- 
                                            <GWC:SwitchControl runat="server" ID="cd6" CssClass="col-xs-12 col-md-2 d-flex align-items-center justify-content-center" OnText="Si" OffText="No" Label="Combinar Descripción"/>
                                            --%>
                                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6" ID="fbx_Cliente" Label="Cliente" OnTextChanged="fbx_Cliente_TextChanged" RequiredSelect="true"/>
                                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6" ID="fbx_Proveedor" Label="Proveedor" OnTextChanged="fbx_Proveedor_TextChanged" RequiredSelect="true"/>                                   
                                        </ListControls>
                                    </GWC:PillboxControl>
                                    <asp:Panel runat="server" CssClass="w-100 fieldset custom_cat_3_content">
                                        <GWC:CatalogControl runat="server" ID="cat_DescipcionesFacturas" CssClass="col custom_cat_3" KeyField="indice">
                                            <Columns>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_IdKrom" Label="Id Krom" Enabled="false"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_NumeroParte" Label="Número de parte"/>
                                                <GWC:InputControl runat="server" ID="txt_Descripcion" Label="Descripción" Type="Text"/>
                                                <GWC:SwitchControl runat="server" ID="sw_AplicaCove" OnText="Si" OffText="No" Label="Aplica COVE"/>
                                                <GWC:InputControl runat="server" ID="txt_DescripcionCove" Label="Descripción COVE" Type="Text"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_Alias" Label="Alias"/>
                                                <GWC:SelectControl runat="server" ID="sl_TipoAlias" Label="Tipo Alias">
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
                                    </asp:Panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl ID="_fshistoriales" runat="server" Label="Historiales" Visible="false">
                            <ListControls>
                                <GWC:FieldsetControl ID="_fshistorialclasificacion" runat="server" Label="Historial de Clasificación" Priority="false">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="cat_HistorialClasificacion" CssClass="w-100" KeyField="indice" UserInteraction="false">
                                            <Columns>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoFraccion" Label="Fracción Arancelaria"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoNico" Label="Fracción Nico"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoMotivo" Label="Motivo"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoFechaModificacion" Label="Fecha Modificacion"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl>
                                <GWC:FieldsetControl ID="_fshistorialdescipciones" runat="server" Label="Historial de Descripciones" Priority="false">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="cat_HistorialDescripciones" CssClass="w-100" KeyField="indice" UserInteraction="false">
                                            <Columns>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoNumeroParte" Label="Número de parte"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoDescripcion" Label="Descripción"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoProveedor" Label="Proveedor"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoCliente" Label="Cliente"/>
                                                <GWC:InputControl Type="Text" runat="server" ID="txt_HistoricoFechaArchivado" Label="Fecha archivado"/>
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
