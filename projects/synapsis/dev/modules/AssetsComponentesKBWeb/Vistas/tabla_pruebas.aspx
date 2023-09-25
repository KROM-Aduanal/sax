<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Home.Master" CodeBehind="tabla_pruebas.aspx.vb" Inherits=".tabla_pruebas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    
    <section class="content">
        <div class="box box-primary">
                <div class="box-header with-border">
                     <i class="fa fa-list"></i><p class="box-title" style="font-size: 20px;">Tabla Pruebas</p>
                    
                </div>

                <div class="box-body">
                    
                    <button id="boton">Agregar fila</button>
                    <table class="table zebra" id="tbl1" is="kbw-table">
                        <template>
                            <button action="/CapaPresentacion/alta_referencia.aspx/BorrarRegistro" delete-element>Borrar</button>
                        </template>
                        <thead>
                            <tr>
                                <th bind="id">ID</th>
                                <th bind="name">Nombre</th>
                                <th bind="username">Nickname</th>
                                <th bind="email">Correo</th>
                                <th bind="phone">Teléfono</th>
                                <th bind="website">Página Web</th>
                                <th bind="company.name">Compañia</th>
                                <th bind="address.city">Lugar</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
    </section>

    <script>
        /*fetch('https://jsonplaceholder.typicode.com/users')
         .then(response => response.json())
         .then((json) => {
     
           tbl1.dataSource = json;

        });

        boton.addEventListener('click', () => {
        
            const row = {
                id: 1,
                name: 'Patricio Estrella',
                username: 'Patrick',
                email: 'patricio@gmail.com',
                phone: '2297886377',
                website: '',
                company: { 
                    name: 'Cangre Burguer'
                },
                address: {
                    city: 'Fondo de Vikini'
                }
            }

            tbl1.tableRows.add(row);
        
        });*/
    </script>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>