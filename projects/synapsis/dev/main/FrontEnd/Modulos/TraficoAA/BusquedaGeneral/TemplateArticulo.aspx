<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TemplateArticulo.aspx.vb" Inherits=".TemplateArticulo" %>
<div class="item-content col-auto d-flex">
    <div class="col-auto">
        <img src="https://cdn.forbes.com.mx/2019/07/medicina-pildoras-pastillas-cancer-640x360.jpg"/>
    </div>
    <div class="col d-flex justify-content-end">
        <div class="col-auto d-flex flex-column p-4 item-content-element">
            <div class="col-12 pl-4 pr-4 d-flex align-items-center">
                <div class="col">
                    {{t_MedioTransporte}}
                </div>
                <div class="col-auto">
                    <a href="" class="star-icon s-open">save</a>
                </div>
            </div>
            <div class="col-12 d-flex pl-4 pr-4">
                <div class="col-4 pr-4">
                    <small>Valor aduana</small><br/>
                    $112,894 MXN<br />
                    <small>Número de parte (PO)</small><br/>
                    {{f_FechaRegistro}}
                </div>
                <div class="col-4 pr-4">
                    <small>Factura</small><br/>
                    GNU345DES<br />
                    <small>Pedimento</small><br />
                    21 43 3931 1000548
                </div>
                <div class="col-4 d-flex align-items-end pl-4">
                    <button>¿Ver más?</button>
                </div>
            </div>
        </div>
    </div>
</div>