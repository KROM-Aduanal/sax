Imports System.Windows.Forms.Form
Imports System.Windows.Forms

Namespace gsol

    Public Class AdaptadorIU
        Implements IAdaptador

#Region "Atributos"

        Private _espacioTrabajo As IEspacioTrabajo
        Private _permisos As Dictionary(Of Int32, String)
        Private _controles As ICollection
        Private _componente As IRevisorComponente
        Private _dominioadaptador As IAdaptador.DominiosAdptador

#End Region

#Region "Constructores"

        Sub New()
            _permisos = New Dictionary(Of Int32, String)
            _controles = New Collection
            _componente = New RevisorComponentes
            _dominioadaptador = IAdaptador.DominiosAdptador.Identificados
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Property Controles As ICollection _
            Implements IAdaptador.Controles
            Get
                Return _controles
            End Get
            Set(value As ICollection)
                _controles = value
            End Set
        End Property

        Public WriteOnly Property EspacioTrabajo As IEspacioTrabajo _
            Implements IAdaptador.EspacioTrabajo
            Set(value As IEspacioTrabajo)
                _espacioTrabajo = value
            End Set
        End Property

        Public Property DominioAdptador As IAdaptador.DominiosAdptador _
            Implements IAdaptador.DominioAdptador
            Get
                Return _dominioadaptador
            End Get
            Set(value As IAdaptador.DominiosAdptador)
                _dominioadaptador = value
            End Set
        End Property

#End Region

#Region "Metodos"

        'Habilita los controles del formulario a los que tenga permiso 
        Public Sub HabilitaComponentes() _
            Implements IAdaptador.HabilitaComponentes

            'Asigna el valor del dominio del adaptador total o identificado
            _componente.DominioAdaptador = _dominioadaptador

            'Obtiene una diccionario de Controles (i_cve_permiso, Nombre del Control)
            _componente.ObtenerControles(_controles)

            'Selecciona segun la aplicacion los componentes que se van a habilitar 
            Select Case _componente.Aplicacion

                Case IRevisorComponente.TipoAplicacion.Escritorio

                    'Recorre los permisos verificando en busca de la clave de los permisos que se verifican conntra la tag de los componentes 
                    For Each sectores_ In _espacioTrabajo.SectorEntorno.Values
                        For Each permisos_ In sectores_.Permisos.Keys
                            Dim control_ = _componente.Control.FirstOrDefault(Function(item) item.Key = permisos_)
                            If control_.Key > 0 Then
                                DirectCast(_controles, ControlCollection).Find(control_.Value, True)(0).Enabled = True
                            End If
                        Next
                    Next

                Case IRevisorComponente.TipoAplicacion.Web

                    Exit Select

            End Select

        End Sub

        Private Sub OntenerPermisos()

            'Crea un diccionario de permisos recorriendo el espacio de trabajo que se recupero
            For Each sectores_ In _espacioTrabajo.SectorEntorno.Values
                For Each permisos_ In sectores_.Permisos.Values
                    _permisos.Add(permisos_.Identificador, permisos_.Descripcion)
                Next
            Next

        End Sub

#End Region

    End Class

End Namespace
