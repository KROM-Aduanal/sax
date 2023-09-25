Namespace gsol

	Public Class IniciaSesion
        Implements IIniciaSesion


#Region "Atributos"

        'Credenciales
        Private _claveUsuario As Int32
        Private _usuario As String
        Private _contrasenia As String
        Private _grupoEmpresarial As Integer
        Private _divisionEmpresarial As Integer
        Private _aplicacion As Integer
        Private _nombreAutenticacion As String
        Private _gruposEmpresariales As DataSet
        Private _estatusAutenticacion As IIniciaSesion.StatusAutenticacion
        Private _lineaBase As LineaBaseIniciaSesion

        Private _claveEjecutivo As Int32

#End Region

#Region "Constructores"

        Sub New()
            'Credenciales
            _claveUsuario = Nothing
            _usuario = Nothing
            _contrasenia = Nothing
            _grupoEmpresarial = Nothing
            _divisionEmpresarial = Nothing
            _aplicacion = Nothing

            _gruposEmpresariales = Nothing
            _nombreAutenticacion = Nothing
            _estatusAutenticacion = IIniciaSesion.StatusAutenticacion.SinDefinir
            _lineaBase = New LineaBaseIniciaSesion
        End Sub

        Sub New(ByVal usuario_ As String, _
                ByRef contrasenia_ As String)

            _usuario = usuario_
            _contrasenia = contrasenia_
            _lineaBase = New LineaBaseIniciaSesion
            _lineaBase.Usuario = usuario_
            _lineaBase.Contrasena = contrasenia_

            _lineaBase.ObtenerGrupos()

            '_claveUsuario = _lineaBase.ClaveUsuario
            '_nombreAutenticacion = _lineaBase.NombreAutenticacion
            _gruposEmpresariales = _lineaBase.GruposEmpresariales
            _estatusAutenticacion = _lineaBase.EstadoAutenticacion

        End Sub

        Sub New(
            ByVal usuario_ As String, _
            ByVal contrasenia_ As String, _
            ByVal grupoEmpresarial_ As Integer, _
            ByVal divisionEmpresarial_ As Integer, _
            ByVal aplicacion_ As Integer
        )
            'Credenciales
            _usuario = usuario_
            _contrasenia = contrasenia_
            _grupoEmpresarial = grupoEmpresarial_
            _divisionEmpresarial = divisionEmpresarial_
            _aplicacion = aplicacion_

            _lineaBase = New LineaBaseIniciaSesion
            _lineaBase.Usuario = usuario_
            _lineaBase.Contrasena = contrasenia_
            _lineaBase.GrupoEmpresarial = grupoEmpresarial_
            _lineaBase.DivisionEmpresarial = divisionEmpresarial_
            _lineaBase.Aplicacion = aplicacion_

            _lineaBase.Autenticar()

            _nombreAutenticacion = _lineaBase.NombreAutenticacion
            _claveEjecutivo = _lineaBase.ClaveEjecutivo
            _estatusAutenticacion = _lineaBase.EstadoAutenticacion
            _claveUsuario = _lineaBase.ClaveUsuario

        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property ClaveEjecutivo As Int32 Implements IIniciaSesion.ClaveEjecutivo
            Get
                Return _claveEjecutivo '_lineaBase.ClaveEjecutivo
            End Get
        End Property

        Public ReadOnly Property EstadoAutenticacion As IIniciaSesion.StatusAutenticacion _
            Implements IIniciaSesion.EstadoAutenticacion
            Get
                Return _estatusAutenticacion
            End Get
        End Property

        Public Property ContrasenaSesion As String _
            Implements IIniciaSesion.IniciaSesionContrasena
            Get
                Return _contrasenia
            End Get
            Set(ByVal value As String)
                _contrasenia = value
                _lineaBase.Contrasena = value
            End Set
        End Property

        Public Property UsuarioSesion As String _
            Implements IIniciaSesion.IniciaSesionUsuario
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
                _lineaBase.Usuario = value
            End Set
        End Property

        Public ReadOnly Property NombreAutenticacion As String _
            Implements IIniciaSesion.NombreAutenticacion
            Get
                Return _nombreAutenticacion
            End Get
        End Property

        Public Property DivisionEmpresarialSesion As Integer _
            Implements IIniciaSesion.IniciaSesionDivisionEmpresarial
            Get
                Return _divisionEmpresarial
            End Get
            Set(ByVal value As Integer)
                _divisionEmpresarial = value
            End Set
        End Property

        Public Property GrupoEmpresarialSesion As Integer _
            Implements IIniciaSesion.IniciaSesionGrupoEmpresarial
            Get
                Return _grupoEmpresarial
            End Get
            Set(ByVal value As Integer)
                _grupoEmpresarial = value
            End Set
        End Property

        Public Property AplicacionSesion As Integer _
            Implements IIniciaSesion.IniciaSesionAplicacion
            Get
                Return _aplicacion
            End Get
            Set(ByVal value As Integer)
                _aplicacion = value
            End Set
        End Property

        Public ReadOnly Property GruposEmpresariales As DataSet _
            Implements IIniciaSesion.GruposEmpresariales
            Get
                Return _gruposEmpresariales
            End Get
        End Property

        Public ReadOnly Property IniciaSesionCalveUsuario As Integer _
            Implements IIniciaSesion.IniciaSesionCalveUsuario
            Get
                Return _claveUsuario
            End Get
        End Property

        Public Property AsignaIniciaSesionClaveUsuario As Integer Implements IIniciaSesion.AsignaIniciaSesionClaveUsuario

            Get
                Return _claveUsuario

            End Get

            Set(value As Integer)

                _claveUsuario = value

            End Set

        End Property

        'Public Property AsignaIniciaSesionClaveUsuario As Integer Implements IIniciaSesion.AsignaIniciaSesionClaveUsuario

#End Region

#Region "Metodos"

        Private Function CargarGruposEmpresariales() As Boolean

            If Not _lineaBase.Usuario And _lineaBase.Contrasena Is Nothing Then
                _lineaBase.ObtenerGrupos()
                _estatusAutenticacion = _lineaBase.EstadoAutenticacion
                Return True
            End If

            Return False
        End Function

        Private Function Autenticar(
        ) As Boolean

            If Not _lineaBase.Usuario And _lineaBase.Contrasena Is Nothing Then

                _lineaBase.Autenticar()
                _estatusAutenticacion = _lineaBase.EstadoAutenticacion
                _nombreAutenticacion = _lineaBase.NombreAutenticacion
                _claveEjecutivo = _lineaBase.ClaveEjecutivo
                _claveUsuario = _lineaBase.ClaveUsuario

            End If

            Return True
        End Function

#End Region



    End Class

End Namespace
