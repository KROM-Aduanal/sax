
Public Class LockButton

#Region "Atributos"

    Private _bloqueado As Boolean

    Private _controlHabilitado As Boolean

    Private _informacionAdicional As String

    Private _tipoComportamientoCodigo As seguridad.ICifrado.TipoComportamientoCodigo

    'Eventos

    Public Event CambioEstado()

    Public Event DespuesBloqueo()

    Public Event DespuesLiberacion()


#End Region

#Region "Constructores"

    Sub New()

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        _bloqueado = True

        _controlHabilitado = True

        _informacionAdicional = "Ingresar contraseña"

        _tipoComportamientoCodigo = seguridad.ICifrado.TipoComportamientoCodigo.MelekDay


    End Sub

#End Region

#Region "Propiedades"

    Public ReadOnly Property Bloqueado As Boolean

        Get
            Return _bloqueado

        End Get

    End Property

    Public Property ControlHabilitado As Boolean

        Get
            Return _controlHabilitado

        End Get
        Set(value As Boolean)

            _controlHabilitado = value

        End Set

    End Property

    Public WriteOnly Property InformacionAdicional As String

        Set(value As String)

            _informacionAdicional = value

        End Set

    End Property

    'NOT IMPLEMENTED
    Public Property ValidaCredencialesUsuario(ByVal _espacioTrabajo As IEspacioTrabajo,
                                              ByVal _permisoEspecial As String) As Boolean
        Get

            Return Nothing

        End Get

        Set(value As Boolean)

        End Set

    End Property

    Public Property TipoComportamientoCodigo As seguridad.ICifrado.TipoComportamientoCodigo
        Get
            Return _tipoComportamientoCodigo
        End Get

        Set(value As seguridad.ICifrado.TipoComportamientoCodigo)
            _tipoComportamientoCodigo = value
        End Set

    End Property

#End Region

#Region "Metodos"

    Private Sub btnBloquear_Click(sender As Object, e As EventArgs) Handles btnBloquear.Click

        'Public Event CambioEstado

        'Public Event DespuesBloqueo

        'Public Event DespuesLiberacion

        RaiseEvent CambioEstado()

        Dim sistema_ As New Organismo

        If btnBloquear.ImageIndex = 1 Then

            'Bloqueado
            btnBloquear.ImageIndex = 0

            _bloqueado = True

            RaiseEvent DespuesBloqueo()

        Else

            Dim password_ As String = Nothing

            If sistema_.GsDialogo(password_,
                                  _informacionAdicional,
                                  SistemaBase.GsDialogo.TipoDialogo.CajaChicaEntrada) = SistemaBase.GsDialogo.Contestacion.Ok Then

                If sistema_.Cifrar.LlaveAccesoAutomatica(_tipoComportamientoCodigo) = password_ Then

                    'Desbloqueado
                    btnBloquear.ImageIndex = 1

                    _bloqueado = False

                    RaiseEvent DespuesLiberacion()

                End If

            End If

        End If

    End Sub

#End Region

End Class
