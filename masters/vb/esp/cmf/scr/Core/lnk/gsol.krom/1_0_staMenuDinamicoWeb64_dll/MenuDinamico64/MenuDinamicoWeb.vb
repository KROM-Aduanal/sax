Imports System.Text
Imports System.Web.Script.Serialization
Imports gsol
Imports System.Reflection

Namespace Gsol.Krom.MenuDinamico

    Public Class MenuDinamicoWeb

#Region "Enums"

        Enum Tema
            Krom
        End Enum

#End Region

#Region "Atributos"

        Private _menuString As StringBuilder

#End Region

#Region "Propiedades"

#End Region

#Region "Constructores"

        Sub New()

            _menuString = New StringBuilder

        End Sub

#End Region

#Region "Métodos"

        ' DEPRECATED
        Public Function ArmarMenuDinamico() As String

            Dim serializer_ As New JavaScriptSerializer()

            Dim menuJSON_ As String = "[{'Descripcion': 'Panel General','Icono': 'fas fa-chart-bar', 'URL': '../../../../../../CapaPresentacion/Ges003-001-Consultas.Principal.aspx'}" &
                                    ", {'Descripcion': 'Consultas','Icono': 'fa fa-laptop','URL': '#', 'Nodo': [{'Descripcion': 'Operaciones', 'URL': '#', 'Tipo': 'treeview-menu', 'Nodo': [{'Descripcion': 'Pagadas', 'URL': '../../../../../../CapaPresentacion/Cat003-001-Consultas_Operaciones.aspx', 'Tipo': 'treeview-menu'}, {'Descripcion': 'Freight Forwarder', 'URL': '../../../../../../CapaPresentacion/Cat005_001_Consultas_OperacionesFreightForwarder.aspx', 'Tipo': 'treeview-menu'}, {'Descripcion': 'Sin pagar', 'URL': '../../../../../../CapaPresentacion/Cat003-001-Consultas_OperacionesVivas.aspx', 'Tipo': 'treeview-menu'}]}]}" &
                                    ", {'Descripcion': 'Administración','Icono': 'fa fa-book','URL': '#', 'Nodo': [{'Descripcion': 'Facturación', 'URL': '#', 'Tipo': 'treeview-menu', 'Nodo': [{'Descripcion': 'Agencias Aduanales', 'URL': '../../../../../../CapaPresentacion/Cat014-001-Consultas_CuentaGastos.aspx', 'Icono': 'fa fa-calculator', 'Tipo': 'treeview-menu'}, {'Descripcion': 'Freight Forwarder', 'URL': '../../../../../../CapaPresentacion/Cat014-001-Consultas_CuentaGastosFreightForwarder.aspx', 'Icono': 'fa fa-calculator', 'Tipo': 'treeview-menu'}]}]}" &
                                    ", {'Descripcion': 'Reportes','Icono': 'fa fa-files-o','URL': '#', 'Nodo': [{'Descripcion': 'Reportes Estándar', 'URL': '#', 'Tipo': 'treeview-menu', 'Nodo': [{'Descripcion': 'Pedimentos', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rpt022-001-Estandar_Pedimentos.aspx', 'Tipo': 'treeview-menu'}" &
                                    ", {'Descripcion': 'Fracciones', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rp022-001-Estandar_Fracciones.aspx', 'Tipo': 'treeview-menu'}" &
                                    ", {'Descripcion': 'Facturas Comerciales', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rpt022-001-Estandar_Facturas.aspx', 'Tipo': 'treeview-menu'}" &
                                    ", {'Descripcion': 'Mercancías', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rpt022-001-Estandar_Mercancias.aspx', 'Tipo': 'treeview-menu'}" &
                                    ", {'Descripcion': 'Facturación Agente Aduanal', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rpt022-001-Estandar_CuentaGastos.aspx', 'Tipo': 'treeview-menu'}" &
                                    ", {'Descripcion': 'Facturación Logística', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rpt022-001-Estandar_CuentaGastosFreightForwarder.aspx', 'Tipo': 'treeview-menu'}" &
                                    ", {'Descripcion': 'Operaciones Logística', 'URL': '../../../../../CapaPresentacion/ReportesEstandar/Rpt022-001-Estandar_OperacionesFreightForwader.aspx', 'Tipo': 'treeview-menu'}]}]}]"

            Dim deserializedResult_ = serializer_.Deserialize(Of List(Of NodoMenuWeb))(menuJSON_)

            Dim menu_ As String = RecorrerArbol(deserializedResult_).ToString

            Return menu_

        End Function

        Public Function CrearJSONMenu(ByVal espacioTrabajo_ As IEspacioTrabajo)

            Dim menuJson_ As String = ""

            Dim contenedor_ As New Object

            If Not espacioTrabajo_.SectorEntorno Is Nothing And
                espacioTrabajo_.SectorEntorno.Count > 0 Then

                For Each sector_ As KeyValuePair(Of Integer, ISectorEntorno) In espacioTrabajo_.SectorEntorno

                    ' IDENTIFICA SI ES UN SECTOR GRÁFICO (MODULO) SE PUEDE OBSERVAR EN LA TABLA CAT000MODULOS CON ID 8
                    If sector_.Value.Identitificador <> 8 Then

                        Continue For

                    End If

                    Dim permisos_ = sector_.Value.Permisos.ToList

                    ' SE RECORREN LOS PERMISOS
                    For Each permiso_ As KeyValuePair(Of Integer, IPermisos) In permisos_

                        ' SI EL PERMISO TIENE DEPENDENCIAS IGNORA ESOS PERMISOS
                        If permiso_.Value.Dependencia <> IConstructorVisual.TipoEntidad.Independiente Then

                            Continue For

                        End If

                        Dim entidad_ As NodoMenuWeb = Me.ObtenerEntidadWeb(permiso_, permisos_)

                        If entidad_ Is Nothing Then

                            Continue For

                        End If

                        menuJson_ = RecorrerArbol(entidad_.Nodo).ToString

                    Next

                Next

            End If

            Return menuJson_

        End Function

        Private Function ObtenerEntidadWeb(ByVal permiso_ As KeyValuePair(Of Integer, IPermisos),
                                           ByVal permisos_ As List(Of KeyValuePair(Of Integer, IPermisos))) As NodoMenuWeb

            'CONSTRUYE LA ENTIDAD
            Dim entidad_ As NodoMenuWeb = Me.CrearControlWeb(permiso_.Value)

            If entidad_ Is Nothing Then

                Return Nothing

            End If

            'OBTIENE SUS DEPENDENCIAS
            Dim dependencias_ As List(Of KeyValuePair(Of Integer, IPermisos)) = permisos_.Where(Function(item) item.Value.Dependencia = permiso_.Value.Identificador).ToList

            'RECORRE LA COLLECCIÓN DE ENTIDADES DEPENDIENTES
            For Each dependencia_ As KeyValuePair(Of Integer, IPermisos) In dependencias_

                'AGREGA LA ENTIDAD DEPENDIENTE, A SU ENTIDAD PADRE CORRESPONDIENTE
                Me.AgregarControlWeb(entidad_, Me.ObtenerEntidadWeb(dependencia_, permisos_))

            Next

            Return entidad_

        End Function

        Private Function CrearControlWeb(ByVal permiso_ As IPermisos) As Object

            Dim control_ As New List(Of Object)

            Dim nodoWeb_ As New NodoMenuWeb

            'RECORRE LA COLECCIÓN DE ENTIDADES
            For Each entidad_ In permiso_.Entidades.Values

                nodoWeb_.ClaveEntidad = entidad_.Entidad

                'RECORRE LA COLECCIÓN DE ATRIBUTOS DE CADA ENTIDAD
                For Each atributo_ In entidad_.Atributos.Values

                    Me.AsignarPropiedadWeb(nodoWeb_, atributo_.Descripcion, atributo_.Valor)

                Next

            Next

            Return nodoWeb_

        End Function

        Private Sub AsignarPropiedadWeb(ByVal entidad_ As NodoMenuWeb,
                                        ByVal nombreAtributo_ As String,
                                        ByVal valorAtributo_ As String)

            Select Case nombreAtributo_

                Case "Text"
                    entidad_.Descripcion = valorAtributo_

                Case "Icon"
                    entidad_.Icono = valorAtributo_

                Case "URL"
                    entidad_.URL = valorAtributo_

                Case "Type"
                    entidad_.Tipo = valorAtributo_

                Case "Order"
                    entidad_.Order = valorAtributo_

            End Select

        End Sub

        ' ASIGNA LA ENTIDAD HIJO AL NODO PADRE
        Private Sub AgregarControlWeb(ByVal entidadPadre_ As NodoMenuWeb,
                                      ByVal entidadHijo_ As NodoMenuWeb)

            If entidadHijo_ Is Nothing Then

                Exit Sub

            End If

            entidadPadre_.Nodo.Add(entidadHijo_)

        End Sub

        Private Sub OrdernarMenu(ByVal entidad_ As NodoMenuWeb)

            entidad_.Nodo = entidad_.Nodo.OrderBy(Function(x) x.Order).ToList

            For Each nodo_ As NodoMenuWeb In entidad_.Nodo

                nodo_.Nodo = nodo_.Nodo.OrderBy(Function(x) x.Order).ToList

                OrdernarMenu(entidad_)

            Next

        End Sub

        ' ARMA EL MENÚ WEB
        Private Function RecorrerArbol(ByVal nodos_ As List(Of NodoMenuWeb)) As StringBuilder

            Dim totalNodos_ As Int16 = nodos_.Count

            Dim count_ As Int16 = 0

            'Ordena los nodos
            nodos_ = nodos_.OrderBy(Function(x) x.Order).ToList()

            For Each nodo_ As NodoMenuWeb In nodos_

                Dim icono_ As String = If(nodo_.Icono = "" Or nodo_.Icono = " ", "fas fa-circle", nodo_.Icono)

                ' Valida si el nodo actual tiene un nodo hijo
                If nodo_.Nodo.Count = 0 Then

                    If nodo_.Tipo = "treeview-menu" Then

                        If count_ = 0 Then

                            _menuString.Append("<ul class='treeview-menu'>")

                        End If

                        _menuString.Append("<li><a href='" + nodo_.URL + "'><i class='" + icono_ + "' style='font-size: 12px;margin-right: 6px;'></i><span>" + nodo_.Descripcion + "</span></a></li>")

                        count_ += 1

                        If count_ = totalNodos_ Then

                            _menuString.Append("</ul>")

                        End If

                    Else

                        _menuString.Append("<li><a href='" + nodo_.URL + "'><i class='" + icono_ + "' style='font-size: 12px;margin-right: 6px;'></i><span>" + nodo_.Descripcion + "</span></a></li>")

                    End If

                Else

                    ' Valida si es un nodo desplegable
                    If nodo_.Tipo = "treeview-menu" Then

                        Dim iconoDesplegable_ As String = ""

                        If count_ = 0 Then

                            _menuString.Append("<ul class='treeview-menu'>")

                        End If

                        If nodo_.URL Is Nothing Or nodo_.URL = "" Or nodo_.URL = "#" Then

                            _menuString.Append("<li class='treeview'>")

                            iconoDesplegable_ = "<span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span>"

                        Else

                            _menuString.Append("<li>")

                        End If

                        _menuString.Append("<a href='" + nodo_.URL + "'><i class='" + icono_ + "' style='font-size: 12px;margin-right: 6px;'></i>" + nodo_.Descripcion + iconoDesplegable_ + "</a>")

                        count_ += 1

                        RecorrerArbol(nodo_.Nodo)

                        _menuString.Append("</li>")

                        If count_ = totalNodos_ Then

                            _menuString.Append("</ul>")

                        End If

                    Else

                        Dim iconoDesplegable_ As String = ""

                        If nodo_.URL Is Nothing Or nodo_.URL = "" Or nodo_.URL = "#" Then

                            _menuString.Append("<li class='treeview'>")

                            iconoDesplegable_ = "<span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span>"

                        Else

                            _menuString.Append("<li>")

                        End If

                        _menuString.Append("<a href='" + nodo_.URL + "'><i class='" + icono_ + "' style='font-size: 12px;margin-right: 6px;'></i><span>" + nodo_.Descripcion + "</span>" + iconoDesplegable_ + "</a>")

                        RecorrerArbol(nodo_.Nodo)

                        _menuString.Append("</li>")

                    End If

                End If

            Next

            Return _menuString

        End Function

#End Region

    End Class

    Public Class NodoMenuWeb

#Region "Atributos"

        Private _claveEntidad As Int32

        Private _descripcion As String

        Private _icono As String

        Private _URL As String

        Private _nodo As List(Of NodoMenuWeb)

        Private _tipo As String

        Private _order As String

#End Region

#Region "Constructores"

        Sub New()

            _descripcion = ""

            _icono = ""

            _URL = Nothing

            _nodo = New List(Of NodoMenuWeb)

            _tipo = ""

            _claveEntidad = Nothing

            _order = Nothing

        End Sub

#End Region

#Region "Propiedades"

        Property ClaveEntidad As Int32

            Get

                Return _claveEntidad

            End Get

            Set(value_ As Int32)

                _claveEntidad = value_

            End Set

        End Property

        Property Descripcion As String

            Get

                Return _descripcion

            End Get

            Set(value_ As String)

                _descripcion = value_

            End Set

        End Property

        Property Icono As String

            Get

                Return _icono

            End Get

            Set(value_ As String)

                _icono = value_

            End Set

        End Property

        Property URL As String

            Get

                Return _URL

            End Get

            Set(value_ As String)

                _URL = value_

            End Set

        End Property

        Property Nodo As List(Of NodoMenuWeb)

            Get

                Return _nodo

            End Get

            Set(value_ As List(Of NodoMenuWeb))

                _nodo = value_

            End Set

        End Property

        Property Tipo As String

            Get

                Return _tipo

            End Get

            Set(value_ As String)

                _tipo = value_

            End Set

        End Property

        Property Order As Int32

            Get

                Return _order

            End Get

            Set(value_ As Int32)

                _order = value_

            End Set

        End Property

#End Region

    End Class

End Namespace
