Namespace gsol

	Public Interface IScript

#Region "Atributos"

		Enum TipoOperacion
			NoDefinida
			Agregar
			Mostrar
			Eliminar
        End Enum

        Enum Campos1
            Campo1 = 1
            Campo2 = 2
            Campo3 = 3
            Campo4 = 4

        End Enum


#End Region

#Region "Propiedades"

		Property Estatus As Boolean
		Property Operacion As IScript.TipoOperacion
		Property Resultado As Object

#End Region

#Region "Métodos"

        Sub Ejecutar(ByVal parametro_ As Object)

        Sub Ejecutar2()

#End Region

	End Interface

End Namespace