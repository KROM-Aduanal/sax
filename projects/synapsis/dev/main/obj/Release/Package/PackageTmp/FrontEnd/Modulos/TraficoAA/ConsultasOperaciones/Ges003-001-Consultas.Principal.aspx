<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="/FrontEnd/Modulos/Home.Master" CodeBehind="Ges003-001-Consultas.Principal.aspx.vb" Inherits="Principal" %>

<%@ Import Namespace="CapaPresentacion" %>
<%@ Import Namespace="CapaEntidades" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentFindbar" runat="server">

    <GWC:FindbarControl ID="sltfindbar" KeyField="id" DisplayField="name" runat="server">
        <Filters>
            <GWC:BarFilter Text="Contenedores" />
            <GWC:BarFilter Text="Facturas" />
        </Filters>
    </GWC:FindbarControl>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">

    <div class="d-flex justify-content-center align-items-center" style="height:80vh">
    
        <h1 style="font-size: 7rem;font-weight:bold;"><font color="#79145c">Bien</font>venidos
            <small style="position: absolute;
                    font-size: 250%;
                    opacity: .15;
                    display: block;
                    top: 45%;
                    left: 50%;
                    transform: translate(-50%,-50%);
                    ">Bienvenidos</small>
        </h1>
    
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>