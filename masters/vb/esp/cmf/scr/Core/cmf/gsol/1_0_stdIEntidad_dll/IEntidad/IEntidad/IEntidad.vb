Namespace gsol

	Public Interface IEntidad

#Region "Atributos"

#End Region

#Region "Constructores"

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

		Property Aplicacion As Int32
		Property Entidad As Int32
		'Cambios a utilizar 
		Property Atributos As Dictionary(Of Int32, IAtributo)
		Property Eventos As Dictionary(Of Int32, IEvento)

#End Region

#Region "Metodos"

#End Region

	End Interface

End Namespace
