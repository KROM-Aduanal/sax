Namespace gsol

	Public Interface IPermisos

#Region "Atributos"

#End Region

#Region "Propiedades"

		Property Identificador As Int32
		Property Descripcion As String
		Property Dependencia As Int32
		Property Componente As Componente
		Property Funcion As Funcion
		Property Acciones As Accion
		Property Entidades As Dictionary(Of Int32, IEntidad)
		Property TipoAplicacion As String
		Property NombreEnsamblado As String
		Property NombreContenedor As String

#End Region

#Region "Metodos"

#End Region

	End Interface

End Namespace

