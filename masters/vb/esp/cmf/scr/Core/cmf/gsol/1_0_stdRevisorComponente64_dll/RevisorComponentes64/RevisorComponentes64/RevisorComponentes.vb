Namespace gsol

    Public Class RevisorComponentes
        Implements IRevisorComponente

#Region "Atributos"

        Private _control As Dictionary(Of Int32, String)
        Private _aplicacion As IRevisorComponente.TipoAplicacion
        Private _dominioadaptador As IAdaptador.DominiosAdptador

#End Region

#Region "Constructores"

        Sub New()
            _control = New Dictionary(Of Int32, String)
            _aplicacion = IRevisorComponente.TipoAplicacion.NoDefinido
            _dominioadaptador = IAdaptador.DominiosAdptador.Identificados
        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public ReadOnly Property Aplicacion As IRevisorComponente.TipoAplicacion _
            Implements IRevisorComponente.Aplicacion
            Get
                Return _aplicacion
            End Get
        End Property

        Public Property Control As Dictionary(Of Integer, String) _
            Implements IRevisorComponente.Control
            Get
                Return _control
            End Get
            Set(value As Dictionary(Of Integer, String))
                _control = value
            End Set
        End Property

        Public Property DominioAdaptador As IAdaptador.DominiosAdptador _
            Implements IRevisorComponente.DominioAdaptador
            Get
                Return _dominioadaptador
            End Get
            Set(value As IAdaptador.DominiosAdptador)
                _dominioadaptador= value
            End Set
        End Property
#End Region

#Region "Metodos"

        Private Function ObtenerTipoAplicacion(
            ByVal ensamblado_ As String
            ) As IRevisorComponente.TipoAplicacion

            Select Case ensamblado_
                Case IRevisorComponente.TipoAplicacion.Escritorio.Descripcion
                    Return IRevisorComponente.TipoAplicacion.Escritorio
                Case IRevisorComponente.TipoAplicacion.Web.Descripcion
                    Return IRevisorComponente.TipoAplicacion.Web
                Case Else
                    Return IRevisorComponente.TipoAplicacion.NoDefinido
            End Select

        End Function

        Public Sub ObtenerControles(ByVal controles_ As ICollection) _
            Implements IRevisorComponente.ObtenerControles

            Me._aplicacion = Me.ObtenerTipoAplicacion(controles_.GetType.Namespace)


            Select Case _aplicacion
                Case IRevisorComponente.TipoAplicacion.Escritorio
                    Select Case _dominioadaptador
                        Case IAdaptador.DominiosAdptador.Identificados
                            ControlesIdentificados(controles_)
                        Case IAdaptador.DominiosAdptador.Total
                            ControlesTotales(controles_)
                    End Select
                Case IRevisorComponente.TipoAplicacion.Web
                    Exit Select
            End Select

        End Sub

        Sub ControlesIdentificados(ByVal controles_ As Object)
            For Each componente_ In controles_
                If componente_.Controls.Count > 0 Then
                    Me.ControlesIdentificados(componente_.Controls)
                End If
                If componente_.Tag Is Nothing Or componente_.Tag = "" Then
                    Continue For
                Else
                    _control.Add(componente_.Tag, componente_.Name)
                    componente_.Enabled = False
                End If
            Next
        End Sub

        Sub ControlesTotales(ByVal controles_ As Object)
            For Each componente_ In controles_
                If componente_.Controls.Count > 0 Then
                    Me.ControlesTotales(componente_.Controls)
                    componente_.Enabled = False
                End If
                If componente_.Tag Is Nothing Or componente_.Tag = "" Then
                    componente_.Enabled = False
                    Continue For
                Else
                    _control.Add(componente_.Tag, componente_.Name)
                    componente_.Enabled = False
                End If
            Next
        End Sub

#End Region

    End Class

End Namespace
