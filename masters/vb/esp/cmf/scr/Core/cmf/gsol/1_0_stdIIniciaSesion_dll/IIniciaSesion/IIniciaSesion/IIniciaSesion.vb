Namespace gsol

	Public Interface IIniciaSesion

#Region "Atributos"

		Enum StatusAutenticacion
			Aceptado = 1
			Rechazado = 2
            SinDefinir = 3
            FalloConexion = 4
            SinDatos = 5
        End Enum

#End Region

#Region "Propiedades"


        Property IniciaSesionUsuario As String
        Property IniciaSesionContrasena As String
        Property IniciaSesionGrupoEmpresarial As Integer
        Property IniciaSesionDivisionEmpresarial As Integer
        Property IniciaSesionAplicacion As Integer
        ReadOnly Property GruposEmpresariales As DataSet
        ReadOnly Property EstadoAutenticacion As StatusAutenticacion
        ReadOnly Property NombreAutenticacion As String
        ReadOnly Property IniciaSesionCalveUsuario As Int32
        Property AsignaIniciaSesionClaveUsuario As Int32

        ReadOnly Property ClaveEjecutivo As Int32

#End Region

#Region "Metodos"

#End Region

	End Interface

End Namespace


