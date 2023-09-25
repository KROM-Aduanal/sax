Namespace gsol

	Public Interface ICredenciales

#Region "Atributos"

		Enum MetodosCredencializacion
			SinDefinir
			Basico
			WebService
		End Enum
		Enum TipoCifrado
			SinDefinir
			Debil
			Base64
			AES128
			AES256
		End Enum

#End Region

#Region "Propiedades"

		Property CredencialUsuario As String
		Property ContraseniaUsuario As String
		Property GrupoEmpresarial As Integer
		Property DivisionEmpresaria As Integer
		Property Aplicacion As Integer
		Property MetodoCredencializacion As MetodosCredencializacion
		Property CifradoCredencializacion As TipoCifrado
		Property LlaveCifrado As String
		ReadOnly Property GruposEmpresariales As DataSet
		ReadOnly Property EstadoAutenticacion As IIniciaSesion.StatusAutenticacion
		ReadOnly Property NombreAutenticacion As String
        ReadOnly Property ClaveUsuario As Int32
        Property AsignarClaveUsuario As Int32

        ReadOnly Property ClaveEjecutivo As Int32

#End Region

#Region "Metodos"

		Sub CargarGruposEmpresariales()
		Sub IniciarSesion()

#End Region

	End Interface

End Namespace

